using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace ProyectoArqSoft.Pages.Cliente
{
    public class ClienteEditModel : PageModel
    {
        private readonly IConfiguration Configuration;

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
        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string Telefono { get; set; }

        public ClienteEditModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = Configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

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
                    string updateQuery = @"UPDATE cliente 
                        SET tipo_cliente = @tipo_cliente, 
                            nombre = @nombre, 
                            ci = @ci, 
                            edad = @edad, 
                            telefono = @telefono 
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
                return Page();
            }
        }
    }
}