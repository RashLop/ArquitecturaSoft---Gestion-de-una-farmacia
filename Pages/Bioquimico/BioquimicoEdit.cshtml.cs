using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoEditModel : PageModel
    {
        [BindProperty]
        public int IdBioquimico { get; set; }

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

        [BindProperty]
        public bool Activo { get; set; }

        private readonly IConfiguration configuration;
        private readonly BioquimicoValidacion _validacion;

        public BioquimicoEditModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            _validacion = new BioquimicoValidacion();
        }

        public IActionResult OnGet(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT idBioquimico, nombres, apellido_paterno, apellido_materno, 
                                            ci, ci_extencion, telefono, activo
                                    FROM bioquimico 
                                    WHERE idBioquimico = @id";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IdBioquimico = reader.GetInt32("idBioquimico");
                                Nombres = reader.GetString("nombres");
                                Apellido_Paterno = reader.GetString("apellido_paterno");
                                Apellido_Materno = reader.GetString("apellido_materno");
                                Ci = reader.GetString("ci");
                                Ci_Extencion = reader.IsDBNull(reader.GetOrdinal("ci_extencion")) ? "LP" : reader.GetString("ci_extencion");
                                Telefono = reader.GetString("telefono");
                                Activo = reader.GetBoolean("activo");
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Bioquímico no encontrado";
                                return RedirectToPage("Bioquimico");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar bioquímico: {ex.Message}";
                return RedirectToPage("Bioquimico");
            }

            return Page();
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

                    string updateQuery = @"UPDATE bioquimico 
                                        SET nombres = @nombres,
                                            apellido_paterno = @apellido_paterno,
                                            apellido_materno = @apellido_materno,
                                            ci = @ci,
                                            ci_extencion = @ci_extencion,
                                            telefono = @telefono,
                                            activo = @activo,
                                            ultima_actualizacion = NOW()
                                        WHERE idBioquimico = @id";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", IdBioquimico);
                        command.Parameters.AddWithValue("@nombres", Nombres?.Trim());
                        command.Parameters.AddWithValue("@apellido_paterno", Apellido_Paterno?.Trim());
                        command.Parameters.AddWithValue("@apellido_materno", Apellido_Materno?.Trim());
                        command.Parameters.AddWithValue("@ci", Ci?.Trim());
                        command.Parameters.AddWithValue("@ci_extencion", Ci_Extencion.Trim().ToUpper());
                        command.Parameters.AddWithValue("@telefono", Telefono?.Trim());
                        command.Parameters.AddWithValue("@activo", Activo);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Bioquímico actualizado exitosamente";
                            return RedirectToPage("Bioquimico");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "No se pudo actualizar el bioquímico";
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
                TempData["ErrorMessage"] = $"Error al actualizar: {ex.Message}";
                return Page();
            }
        }
    }
}