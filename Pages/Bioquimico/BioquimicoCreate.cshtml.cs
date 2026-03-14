using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoCreateModel : PageModel
    {
        [BindProperty]
        public string Nombres { get; set; }

        [BindProperty]
        public string Apellido_Paterno { get; set; }

        [BindProperty]
        public string Apellido_Materno { get; set; }

        [BindProperty]
        public string Ci { get; set; }

        [BindProperty]
        public string Ci_Extencion { get; set; }

        [BindProperty]
        public string Telefono { get; set; }

        private readonly IConfiguration configuration;
        private readonly BioquimicoValidacion _validacion;

        public BioquimicoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            _validacion = new BioquimicoValidacion();
        }

        public void OnGet()
        {
            Ci_Extencion = "LP";
        }

        public IActionResult OnPost()
        {
            var bioquimico = new ProyectoArqSoft.Models.Bioquimico
            {
                Nombres = Nombres,
                Apellido_Paterno = Apellido_Paterno,
                Apellido_Materno = Apellido_Materno,
                Ci = Ci,
                Ci_Extencion = Ci_Extencion,
                Telefono = Telefono
            };

            if (!_validacion.EsValido(bioquimico))
            {
                TempData["ErrorMessage"] = _validacion.ObtenerMensajesError();
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Ci_Extencion))
            {
                Ci_Extencion = "LP";
            }

            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = @"INSERT INTO bioquimico 
                        (nombres, apellido_paterno, apellido_materno, ci, ci_extencion, telefono, activo, fecha_registro) 
                        VALUES 
                        (@nombres, @apellido_paterno, @apellido_materno, @ci, @ci_extencion, @telefono, 1, NOW())";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nombres", Nombres?.Trim());
                        command.Parameters.AddWithValue("@apellido_paterno", Apellido_Paterno?.Trim());
                        command.Parameters.AddWithValue("@apellido_materno", Apellido_Materno?.Trim());
                        command.Parameters.AddWithValue("@ci", Ci?.Trim());
                        command.Parameters.AddWithValue("@ci_extencion", Ci_Extencion.Trim().ToUpper());
                        command.Parameters.AddWithValue("@telefono", Telefono?.Trim());

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Bioquímico registrado exitosamente";
                            return RedirectToPage("Bioquimico");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "No se pudo registrar el bioquímico";
                            return Page();
                        }
                    }
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                TempData["ErrorMessage"] = "Ya existe un bioquímico con ese CI";
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al registrar: {ex.Message}";
                return Page();
            }
        }
    }
}