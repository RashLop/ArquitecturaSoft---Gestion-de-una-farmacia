using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

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

        public IActionResult OnPost()
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(Tipo_Cliente) || string.IsNullOrEmpty(Nombre) ||
                string.IsNullOrEmpty(Carnet) || Edad <= 0 || string.IsNullOrEmpty(Telefono))
            {
                TempData["ErrorMessage"] = "Todos los campos son requeridos";
                return Page();
            }

            string connectionString = Configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar si el carnet ya existe
                    string checkQuery = "SELECT COUNT(*) FROM cliente WHERE ci = @ci AND estado = 1";
                    MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@ci", Carnet);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        TempData["ErrorMessage"] = "Ya existe un cliente con ese carnet";
                        return Page();
                    }

                    // Insertar nuevo cliente
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