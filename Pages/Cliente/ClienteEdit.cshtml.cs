using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace ProyectoArqSoft.Pages
{
    public class ClienteEditModel : PageModel
    {
        private readonly IConfiguration Configuration;

        public ClienteEditModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        public string Tipo_Cliente { get; set; }

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Carnet { get; set; }

        [BindProperty]
        public int Edad { get; set; }

        [BindProperty]
        public string Telefono { get; set; }

        public IActionResult OnGet(int id)
        {
            Console.WriteLine($"=== INTENTANDO EDITAR CLIENTE ID: {id} ===");

            string connectionString = Configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT idCliente, tipo_cliente, nombre, ci, edad, telefono 
                                   FROM cliente 
                                   WHERE idCliente = @id";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IdCliente = reader.GetInt32("idCliente");
                            Tipo_Cliente = reader.GetString("tipo_cliente");
                            Nombre = reader.GetString("nombre");
                            Carnet = reader.GetString("ci");
                            Edad = reader.GetInt32("edad");
                            Telefono = reader.GetString("telefono");

                            Console.WriteLine($"✓ Cliente encontrado: {Nombre}");
                        }
                        else
                        {
                            Console.WriteLine($"✗ Cliente con ID {id} NO encontrado");
                            TempData["ErrorMessage"] = "Cliente no encontrado";
                            return RedirectToPage("Cliente");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error: {ex.Message}");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToPage("Cliente");
            }

            return Page();
        }
        private bool ValidarNombre(string nombre)
        {
            return !string.IsNullOrEmpty(nombre) &&
                   nombre.Length >= 3 &&
                   nombre.Length <= 100 &&
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
            Console.WriteLine("=== GUARDANDO CAMBIOS DE CLIENTE ===");

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
                TempData["ErrorMessage"] = "El cliente debe ser mayor de 18 años";
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
            if (string.IsNullOrEmpty(Tipo_Cliente))
            {
                TempData["ErrorMessage"] = "Debe seleccionar un tipo de cliente";
                return Page();
            }
            string connectionString = Configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM cliente WHERE idCliente = @id";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@id", IdCliente);
                    int existe = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (existe == 0)
                    {
                        TempData["ErrorMessage"] = "Cliente no encontrado en la base de datos";
                        return RedirectToPage("Cliente");
                    }

                    string checkCarnetQuery = "SELECT COUNT(*) FROM cliente WHERE ci = @ci AND idCliente != @id";
                    MySqlCommand checkCarnetCmd = new MySqlCommand(checkCarnetQuery, connection);
                    checkCarnetCmd.Parameters.AddWithValue("@ci", Carnet);
                    checkCarnetCmd.Parameters.AddWithValue("@id", IdCliente);
                    int carnetCount = Convert.ToInt32(checkCarnetCmd.ExecuteScalar());

                    if (carnetCount > 0)
                    {
                        TempData["ErrorMessage"] = "Ya existe otro cliente con ese carnet";
                        return Page();
                    }
                    string updateQuery = @"UPDATE cliente 
                        SET tipo_cliente = @tipo_cliente, 
                            nombre = @nombre, 
                            ci = @ci, 
                            edad = @edad, 
                            telefono = @telefono 
                        WHERE idCliente = @id";

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@id", IdCliente);
                    updateCmd.Parameters.AddWithValue("@tipo_cliente", Tipo_Cliente);
                    updateCmd.Parameters.AddWithValue("@nombre", Nombre);
                    updateCmd.Parameters.AddWithValue("@ci", Carnet);
                    updateCmd.Parameters.AddWithValue("@edad", Edad);
                    updateCmd.Parameters.AddWithValue("@telefono", Telefono);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        TempData["SuccessMessage"] = "Cliente actualizado exitosamente";
                        return RedirectToPage("Cliente");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No se pudo actualizar el cliente";
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar: " + ex.Message;
                return Page();
            }
        }
    }
}