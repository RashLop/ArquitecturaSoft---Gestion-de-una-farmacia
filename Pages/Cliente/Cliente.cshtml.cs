using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class ClienteModel : PageModel
    {
        public DataTable Clientes_DataTable { get; set; } = new DataTable();

        [BindProperty(SupportsGet = true)]
        public string TipoBusqueda { get; set; }

        [BindProperty(SupportsGet = true)]
        public string TextoBusqueda { get; set; }

        public string Mensaje { get; set; } = "";

        private readonly IConfiguration configuration;

        public ClienteModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet()
        {
            CargarClientes();

            if (!string.IsNullOrEmpty(TipoBusqueda) && !string.IsNullOrEmpty(TextoBusqueda))
            {
                BuscarCliente();
            }
        }

        void CargarClientes()
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT idCliente, 
                                    CONCAT(nombre, ' ', apellido_paterno, ' ', apellido_materno) as nombre_completo,
                                    tipo_cliente, 
                                    CONCAT(ci, ' ', ci_extencion) as ci_completo,
                                    ci,
                                    ci_extencion,
                                    fecha_de_nacimiento,
                                    TIMESTAMPDIFF(YEAR, fecha_de_nacimiento, CURDATE()) as edad,
                                    telefono
                            FROM cliente
                            WHERE estado = 1
                            ORDER BY nombre, apellido_paterno";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
                    adapter.Fill(Clientes_DataTable);
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al cargar clientes: " + ex.Message;
            }
        }

        void BuscarCliente()
        {
            if (string.IsNullOrEmpty(TextoBusqueda) || string.IsNullOrEmpty(TipoBusqueda))
            {
                return;
            }

            string texto = TextoBusqueda.Trim();
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = "";

            switch (TipoBusqueda)
            {
                case "nombre":
                    query = @"SELECT idCliente, 
                                    CONCAT(nombre, ' ', apellido_paterno, ' ', apellido_materno) as nombre_completo,
                                    tipo_cliente, 
                                    CONCAT(ci, ' ', ci_extencion) as ci_completo,
                                    ci,
                                    ci_extencion,
                                    fecha_de_nacimiento,
                                    TIMESTAMPDIFF(YEAR, fecha_de_nacimiento, CURDATE()) as edad,
                                    telefono
                            FROM cliente
                            WHERE estado = 1 AND (nombre LIKE @dato OR apellido_paterno LIKE @dato OR apellido_materno LIKE @dato)
                            ORDER BY nombre, apellido_paterno";
                    break;
                case "ci":
                    query = @"SELECT idCliente, 
                                    CONCAT(nombre, ' ', apellido_paterno, ' ', apellido_materno) as nombre_completo,
                                    tipo_cliente, 
                                    CONCAT(ci, ' ', ci_extencion) as ci_completo,
                                    ci,
                                    ci_extencion,
                                    fecha_de_nacimiento,
                                    TIMESTAMPDIFF(YEAR, fecha_de_nacimiento, CURDATE()) as edad,
                                    telefono
                            FROM cliente
                            WHERE estado = 1 AND (ci LIKE @dato OR ci_extencion LIKE @dato)
                            ORDER BY nombre, apellido_paterno";
                    break;
                case "telefono":
                    query = @"SELECT idCliente, 
                                    CONCAT(nombre, ' ', apellido_paterno, ' ', apellido_materno) as nombre_completo,
                                    tipo_cliente, 
                                    CONCAT(ci, ' ', ci_extencion) as ci_completo,
                                    ci,
                                    ci_extencion,
                                    fecha_de_nacimiento,
                                    TIMESTAMPDIFF(YEAR, fecha_de_nacimiento, CURDATE()) as edad,
                                    telefono
                            FROM cliente
                            WHERE estado = 1 AND telefono LIKE @dato
                            ORDER BY nombre, apellido_paterno";
                    break;
                default:
                    return;
            }

            Clientes_DataTable.Rows.Clear();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("@dato", "%" + texto + "%");
                    MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
                    adapter.Fill(Clientes_DataTable);
                }

                if (Clientes_DataTable.Rows.Count == 0)
                {
                    Mensaje = "No se encontraron clientes";
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error en la búsqueda: " + ex.Message;
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "UPDATE cliente SET estado = 0 WHERE idCliente = @id";
                    MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@id", id);
                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        TempData["ErrorMessage"] = "Cliente no encontrado";
                        return RedirectToPage();
                    }

                    TempData["SuccessMessage"] = "Cliente eliminado exitosamente";
                }
            }
            catch (MySqlException ex) when (ex.Number == 1451)
            {
                TempData["ErrorMessage"] = "No se puede eliminar: cliente con ventas asociadas";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}