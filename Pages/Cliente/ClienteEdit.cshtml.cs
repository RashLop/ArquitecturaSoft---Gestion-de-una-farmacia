using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

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

                            Console.WriteLine($"✓ Cliente encontrado:");
                            Console.WriteLine($"  - Nombre: {Nombre}");
                            Console.WriteLine($"  - Carnet: {Carnet}");
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

        public IActionResult OnPost()
        {
            Console.WriteLine("=== GUARDANDO CAMBIOS ===");
            Console.WriteLine($"ID: {IdCliente}");
            Console.WriteLine($"Tipo_Cliente: {Tipo_Cliente}");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Carnet: {Carnet}");
            Console.WriteLine($"Edad: {Edad}");
            Console.WriteLine($"Telefono: {Telefono}");

            // Validaciones
            if (string.IsNullOrEmpty(Tipo_Cliente))
            {
                TempData["ErrorMessage"] = "El tipo de cliente es requerido";
                return Page();
            }
            if (string.IsNullOrEmpty(Nombre))
            {
                TempData["ErrorMessage"] = "El nombre es requerido";
                return Page();
            }
            if (string.IsNullOrEmpty(Carnet))
            {
                TempData["ErrorMessage"] = "El carnet es requerido";
                return Page();
            }
            if (Edad <= 0 || Edad > 120)
            {
                TempData["ErrorMessage"] = "La edad debe ser entre 1 y 120";
                return Page();
            }
            if (string.IsNullOrEmpty(Telefono))
            {
                TempData["ErrorMessage"] = "El teléfono es requerido";
                return Page();
            }

            string connectionString = Configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar que el cliente existe ANTES de actualizar
                    string checkQuery = "SELECT COUNT(*) FROM cliente WHERE idCliente = @id";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@id", IdCliente);
                    int existe = Convert.ToInt32(checkCmd.ExecuteScalar());

                    Console.WriteLine($"Cliente existe en BD: {existe}");

                    if (existe == 0)
                    {
                        Console.WriteLine("✗ ERROR: Cliente no encontrado en BD");
                        TempData["ErrorMessage"] = "Cliente no encontrado en la base de datos";
                        return RedirectToPage("Cliente");
                    }

                    // Verificar carnet duplicado
                    string checkCarnetQuery = "SELECT COUNT(*) FROM cliente WHERE ci = @ci AND idCliente != @id";
                    MySqlCommand checkCarnetCmd = new MySqlCommand(checkCarnetQuery, connection);
                    checkCarnetCmd.Parameters.AddWithValue("@ci", Carnet);
                    checkCarnetCmd.Parameters.AddWithValue("@id", IdCliente);
                    int carnetCount = Convert.ToInt32(checkCarnetCmd.ExecuteScalar());

                    Console.WriteLine($"Carnet duplicado: {carnetCount}");

                    if (carnetCount > 0)
                    {
                        Console.WriteLine("✗ ERROR: Carnet duplicado");
                        TempData["ErrorMessage"] = "Ya existe otro cliente con ese carnet";
                        return Page();
                    }

                    // Actualizar
                    string updateQuery = @"UPDATE cliente 
                        SET tipo_cliente = @tipo_cliente, 
                            nombre = @nombre, 
                            ci = @ci, 
                            edad = @edad, 
                            telefono = @telefono 
                        WHERE idCliente = @id";

                    Console.WriteLine("Ejecutando UPDATE...");

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@id", IdCliente);
                    updateCmd.Parameters.AddWithValue("@tipo_cliente", Tipo_Cliente);
                    updateCmd.Parameters.AddWithValue("@nombre", Nombre);
                    updateCmd.Parameters.AddWithValue("@ci", Carnet);
                    updateCmd.Parameters.AddWithValue("@edad", Edad);
                    updateCmd.Parameters.AddWithValue("@telefono", Telefono);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    Console.WriteLine($"Filas afectadas: {rowsAffected}");

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("✓ Cliente actualizado correctamente");
                        TempData["SuccessMessage"] = "Cliente actualizado exitosamente";
                        return RedirectToPage("Cliente");
                    }
                    else
                    {
                        Console.WriteLine("✗ No se actualizó ninguna fila");
                        TempData["ErrorMessage"] = "No se pudo actualizar el cliente";
                        return Page();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ ERROR: {ex.Message}");
                TempData["ErrorMessage"] = "Error al actualizar: " + ex.Message;
                return Page();
            }
        }
    }
}