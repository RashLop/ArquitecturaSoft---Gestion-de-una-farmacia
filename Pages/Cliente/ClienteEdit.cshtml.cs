using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace ProyectoArqSoft.Pages
{
    public class ClienteEditModel : PageModel
    {
        [BindProperty]
        public int IdCliente { get; set; }

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

        public ClienteEditModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT idCliente, tipo_cliente, nombre, apellido_paterno, apellido_materno, 
                                            ci, ci_extencion, fecha_de_nacimiento, telefono 
                                    FROM cliente 
                                    WHERE idCliente = @id AND estado = 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IdCliente = reader.GetInt32("idCliente");
                                Tipo_Cliente = reader.GetString("tipo_cliente");
                                Nombre = reader.GetString("nombre");
                                Apellido_Paterno = reader.GetString("apellido_paterno");
                                Apellido_Materno = reader.GetString("apellido_materno");
                                Ci = reader.GetString("ci");
                                Ci_Extencion = reader.IsDBNull(reader.GetOrdinal("ci_extencion")) ? "LP" : reader.GetString("ci_extencion");
                                Fecha_De_Nacimiento = reader.GetDateTime("fecha_de_nacimiento");
                                Telefono = reader.GetString("telefono");
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Cliente no encontrado";
                                return RedirectToPage("Cliente");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar cliente: {ex.Message}";
                return RedirectToPage("Cliente");
            }

            return Page();
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

                    string updateQuery = @"UPDATE cliente 
                                        SET tipo_cliente = @tipo_cliente,
                                            nombre = @nombre,
                                            apellido_paterno = @apellido_paterno,
                                            apellido_materno = @apellido_materno,
                                            ci = @ci,
                                            ci_extencion = @ci_extencion,
                                            fecha_de_nacimiento = @fecha_de_nacimiento,
                                            telefono = @telefono,
                                            ultima_actualizacion = NOW()
                                        WHERE idCliente = @id";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", IdCliente);
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
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                TempData["ErrorMessage"] = "Ya existe un cliente con ese CI";
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