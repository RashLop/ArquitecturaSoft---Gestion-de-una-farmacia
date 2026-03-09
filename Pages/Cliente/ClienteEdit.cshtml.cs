<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace ProyectoArqSoft.Pages
=======
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace ProyectoArqSoft.Pages.Cliente
>>>>>>> 23851e3e2c1bddc3014d5a342e12d51d3f5c7db5
{
    public class ClienteEditModel : PageModel
    {
        private readonly IConfiguration Configuration;

<<<<<<< HEAD
=======
        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El tipo de cliente es requerido")]
        public string Tipo_Cliente { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El carnet es requerido")]
        [Display(Name = "Carnet de Identidad")]
        public string Carnet { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La edad es requerida")]
        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120")]
        public string Edad { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El tel�fono es requerido")]
        [Phone(ErrorMessage = "Formato de tel�fono inv�lido")]
        public string Telefono { get; set; }

>>>>>>> 23851e3e2c1bddc3014d5a342e12d51d3f5c7db5
        public ClienteEditModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

<<<<<<< HEAD
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
=======
        public IActionResult OnGet(int id)
        {
            string connectionString = Configuration.GetConnectionString("MySqlConnection")!;
            string query = "SELECT id, tipo_cliente, nombre, ci, edad, telefono FROM cliente WHERE id = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Id = reader.GetInt32("id");
                        Tipo_Cliente = reader.GetString("tipo_cliente");
                        Nombre = reader.GetString("nombre");
                        Carnet = reader.GetString("ci");
                        Edad = reader.GetString("edad");
                        Telefono = reader.GetString("telefono");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Cliente no encontrado";
                        return RedirectToPage("./Cliente");
                    }
                }
            }
>>>>>>> 23851e3e2c1bddc3014d5a342e12d51d3f5c7db5

            return Page();
        }

        public IActionResult OnPost()
        {
<<<<<<< HEAD
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
=======
            if (!ModelState.IsValid)
            {
>>>>>>> 23851e3e2c1bddc3014d5a342e12d51d3f5c7db5
                return Page();
            }

            string connectionString = Configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

<<<<<<< HEAD
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
=======
                    // Verificar si el cliente existe
                    string checkExistQuery = "SELECT COUNT(*) FROM cliente WHERE id = @id";
                    MySqlCommand checkExistCommand = new MySqlCommand(checkExistQuery, connection);
                    checkExistCommand.Parameters.AddWithValue("@id", Id);
                    int existCount = Convert.ToInt32(checkExistCommand.ExecuteScalar());

                    if (existCount == 0)
                    {
                        TempData["ErrorMessage"] = "Cliente no encontrado";
                        return RedirectToPage("./Cliente");
                    }

                    // Verificar si el carnet ya existe en OTRO cliente
                    string checkCarnetQuery = "SELECT COUNT(*) FROM cliente WHERE ci = @ci AND id != @id";
                    MySqlCommand checkCarnetCommand = new MySqlCommand(checkCarnetQuery, connection);
                    checkCarnetCommand.Parameters.AddWithValue("@ci", Carnet);
                    checkCarnetCommand.Parameters.AddWithValue("@id", Id);
                    int carnetCount = Convert.ToInt32(checkCarnetCommand.ExecuteScalar());

                    if (carnetCount > 0)
                    {
                        ModelState.AddModelError("Carnet", "Ya existe otro cliente con ese carnet");
                        return Page();
                    }

                    // Actualizar cliente
>>>>>>> 23851e3e2c1bddc3014d5a342e12d51d3f5c7db5
                    string updateQuery = @"UPDATE cliente 
                        SET tipo_cliente = @tipo_cliente, 
                            nombre = @nombre, 
                            ci = @ci, 
                            edad = @edad, 
                            telefono = @telefono 
<<<<<<< HEAD
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
=======
                        WHERE id = @id";

                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@id", Id);
                    updateCommand.Parameters.AddWithValue("@tipo_cliente", Tipo_Cliente);
                    updateCommand.Parameters.AddWithValue("@nombre", Nombre);
                    updateCommand.Parameters.AddWithValue("@ci", Carnet);
                    updateCommand.Parameters.AddWithValue("@edad", Edad);
                    updateCommand.Parameters.AddWithValue("@telefono", Telefono);

                    updateCommand.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Cliente actualizado exitosamente";
                return RedirectToPage("./Cliente");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el cliente: " + ex.Message);
>>>>>>> 23851e3e2c1bddc3014d5a342e12d51d3f5c7db5
                return Page();
            }
        }
    }
}