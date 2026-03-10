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
            string query = @"SELECT idCliente, nombre, tipo_cliente, ci, edad, telefono
                            FROM cliente
                            WHERE estado = 1
                            ORDER BY nombre";

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

            if ((TipoBusqueda == "ci" || TipoBusqueda == "telefono") && !EsNumero(texto))
            {
                Mensaje = "Criterio inválido";
                Clientes_DataTable.Rows.Clear();
                return;
            }

            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = "";

            switch (TipoBusqueda)
            {
                case "nombre":
                    query = @"SELECT idCliente, nombre, tipo_cliente, ci, edad, telefono
                            FROM cliente
                            WHERE estado = 1 AND nombre LIKE @dato
                            ORDER BY nombre";
                    break;
                case "ci":
                    query = @"SELECT idCliente, nombre, tipo_cliente, ci, edad, telefono
                            FROM cliente
                            WHERE estado = 1 AND ci LIKE @dato
                            ORDER BY nombre";
                    break;
                case "telefono":
                    query = @"SELECT idCliente, nombre, tipo_cliente, ci, edad, telefono
                            FROM cliente
                            WHERE estado = 1 AND telefono LIKE @dato
                            ORDER BY nombre";
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

        bool EsNumero(string texto)
        {
            foreach (char c in texto)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public IActionResult OnPostDelete(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); 
                    // Soft delete
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