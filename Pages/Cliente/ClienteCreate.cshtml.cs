using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace ProyectoArqSoft.Pages
{
    public class ClienteCreateModel : PageModel
    {
        [BindProperty]
        public string Tipo_Cliente { get; set; }

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Apellido_Paterno { get; set; }

        [BindProperty]
        public string Apellido_Materno { get; set; }

        [BindProperty]
        public string Ci { get; set; }

        [BindProperty]
        public string Ci_Extencion { get; set; }

        [BindProperty]
        public DateTime Fecha_De_Nacimiento { get; set; }

        [BindProperty]
        public string Telefono { get; set; }

        private readonly IConfiguration configuration;

        public ClienteCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet()
        {
            Fecha_De_Nacimiento = DateTime.Today.AddYears(-18);
            Ci_Extencion = "LP";
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Tipo_Cliente))
            {
                TempData["ErrorMessage"] = "El tipo de cliente es requerido";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Nombre))
            {
                TempData["ErrorMessage"] = "El nombre es requerido";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Apellido_Paterno))
            {
                TempData["ErrorMessage"] = "El apellido paterno es requerido";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Apellido_Materno))
            {
                TempData["ErrorMessage"] = "El apellido materno es requerido";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Ci))
            {
                TempData["ErrorMessage"] = "El carnet es requerido";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Ci_Extencion))
            {
                Ci_Extencion = "LP";
            }

            if (Fecha_De_Nacimiento == DateTime.MinValue)
            {
                TempData["ErrorMessage"] = "La fecha de nacimiento es requerida";
                return Page();
            }

            var edad = DateTime.Today.Year - Fecha_De_Nacimiento.Year;
            if (Fecha_De_Nacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;

            if (edad < 18)
            {
                TempData["ErrorMessage"] = "El cliente debe ser mayor de 18 años";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Telefono))
            {
                TempData["ErrorMessage"] = "El teléfono es requerido";
                return Page();
            }

            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = @"INSERT INTO cliente 
                        (tipo_cliente, nombre, apellido_paterno, apellido_materno, ci, ci_extencion, fecha_de_nacimiento, telefono, estado, fecha_registro) 
                        VALUES 
                        (@tipo_cliente, @nombre, @apellido_paterno, @apellido_materno, @ci, @ci_extencion, @fecha_de_nacimiento, @telefono, 1, NOW())";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@tipo_cliente", Tipo_Cliente?.Trim());
                        command.Parameters.AddWithValue("@nombre", Nombre?.Trim());
                        command.Parameters.AddWithValue("@apellido_paterno", Apellido_Paterno?.Trim());
                        command.Parameters.AddWithValue("@apellido_materno", Apellido_Materno?.Trim());
                        command.Parameters.AddWithValue("@ci", Ci?.Trim());
                        command.Parameters.AddWithValue("@ci_extencion", Ci_Extencion.Trim().ToUpper());
                        command.Parameters.AddWithValue("@fecha_de_nacimiento", Fecha_De_Nacimiento);
                        command.Parameters.AddWithValue("@telefono", Telefono?.Trim());

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Cliente registrado exitosamente";
                            return RedirectToPage("Cliente");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "No se pudo registrar el cliente";
                            return Page();
                        }
                    }
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                TempData["ErrorMessage"] = "Ya existe un cliente con ese CI";
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