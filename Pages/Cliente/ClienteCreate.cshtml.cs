using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace ProyectoArqSoft.Pages
{
    public class ClienteCreateModel : PageModel
    {
        private readonly IConfiguration Configuration;

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Tipo_Cliente { get; set; }

        [BindProperty]
        public string Carnet { get; set; }

        [BindProperty]
        public int Edad { get; set; }

        [BindProperty]
        public string Telefono { get; set; }

        public ClienteCreateModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        private bool ValidarNombre(string nombre)
        {
            return !string.IsNullOrEmpty(nombre) &&
                   nombre.Length >= 3 &&
                   Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");
        }

        private bool ValidarCarnet(string carnet)
        {
            return !string.IsNullOrEmpty(carnet) &&
                   carnet.Length >= 5 &&
                   carnet.Length <= 20 &&
                   Regex.IsMatch(carnet, @"^\d+$");
        }

        private bool ValidarTelefono(string telefono)
        {
            return !string.IsNullOrEmpty(telefono) &&
                   telefono.Length >= 7 &&
                   telefono.Length <= 20 &&
                   Regex.IsMatch(telefono, @"^[\d\s\+\-\(\)]+$");
        }

        public IActionResult OnPost()
        {
            if (!ValidarNombre(Nombre))
            {
                TempData["ErrorMessage"] = "El nombre solo puede contener letras y espacios, mínimo 3 caracteres";
                return Page();
            }

            if (!ValidarCarnet(Carnet))
            {
                TempData["ErrorMessage"] = "El carnet solo puede contener números y debe tener entre 5 y 20 dígitos";
                return Page();
            }

            if (Edad < 18)
            {
                TempData["ErrorMessage"] = "El cliente debe ser mayor de 18 años para registrarse";
                return Page();
            }

            if (Edad > 120)
            {
                TempData["ErrorMessage"] = "La edad no puede ser mayor a 120 años";
                return Page();
            }

            if (!ValidarTelefono(Telefono))
            {
                TempData["ErrorMessage"] = "El teléfono debe tener entre 7 y 20 caracteres válidos (números, +, -, (), espacios)";
                return Page();
            }

            string connectionString = Configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM cliente WHERE ci = @ci";
                    MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@ci", Carnet);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        TempData["ErrorMessage"] = "Ya existe un cliente con ese carnet";
                        return Page();
                    }

                    string insertQuery = @"INSERT INTO cliente (tipo_cliente, nombre, ci, edad, telefono, estado)
                                         VALUES(@tipo_cliente, @nombre, @ci, @edad, @telefono, 1)";

                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@tipo_cliente", Tipo_Cliente);
                    insertCommand.Parameters.AddWithValue("@nombre", Nombre);
                    insertCommand.Parameters.AddWithValue("@ci", Carnet);
                    insertCommand.Parameters.AddWithValue("@edad", Edad);
                    insertCommand.Parameters.AddWithValue("@telefono", Telefono);

                    insertCommand.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Cliente creado exitosamente";
                return RedirectToPage("Cliente");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al crear el cliente: " + ex.Message;
                return Page();
            }
        }
    }
}