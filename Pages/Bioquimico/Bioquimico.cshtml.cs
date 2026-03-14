using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoModel : PageModel
    {
        public DataTable Bioquimicos_DataTable { get; set; } = new DataTable();

        [BindProperty(SupportsGet = true)]
        public string TipoBusqueda { get; set; }

        [BindProperty(SupportsGet = true)]
        public string TextoBusqueda { get; set; }

        public string Mensaje { get; set; } = "";

        private readonly IConfiguration configuration;

        public BioquimicoModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet()
        {
            CargarBioquimicos();

            if (!string.IsNullOrEmpty(TipoBusqueda) && !string.IsNullOrEmpty(TextoBusqueda))
            {
                BuscarBioquimico();
            }
        }

        void CargarBioquimicos()
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT idBioquimico, 
                                    CONCAT(nombres, ' ', apellido_paterno, ' ', apellido_materno) as nombre_completo,
                                    CONCAT(ci, ' ', ci_extencion) as ci_completo,
                                    ci,
                                    ci_extencion,
                                    telefono,
                                    activo
                            FROM bioquimico
                            WHERE activo = 1
                            ORDER BY nombres, apellido_paterno";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
                    adapter.Fill(Bioquimicos_DataTable);
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al cargar bioquímicos: " + ex.Message;
            }
        }

        void BuscarBioquimico()
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
                    query = @"SELECT idBioquimico, 
                                    CONCAT(nombres, ' ', apellido_paterno, ' ', apellido_materno) as nombre_completo,
                                    CONCAT(ci, ' ', ci_extencion) as ci_completo,
                                    ci,
                                    ci_extencion,
                                    telefono,
                                    activo
                            FROM bioquimico
                            WHERE activo = 1 AND (nombres LIKE @dato OR apellido_paterno LIKE @dato OR apellido_materno LIKE @dato)
                            ORDER BY nombres, apellido_paterno";
                    break;
                case "ci":
                    query = @"SELECT idBioquimico, 
                                    CONCAT(nombres, ' ', apellido_paterno, ' ', apellido_materno) as nombre_completo,
                                    CONCAT(ci, ' ', ci_extencion) as ci_completo,
                                    ci,
                                    ci_extencion,
                                    telefono,
                                    activo
                            FROM bioquimico
                            WHERE activo = 1 AND (ci LIKE @dato OR ci_extencion LIKE @dato)
                            ORDER BY nombres, apellido_paterno";
                    break;
                case "telefono":
                    query = @"SELECT idBioquimico, 
                                    CONCAT(nombres, ' ', apellido_paterno, ' ', apellido_materno) as nombre_completo,
                                    CONCAT(ci, ' ', ci_extencion) as ci_completo,
                                    ci,
                                    ci_extencion,
                                    telefono,
                                    activo
                            FROM bioquimico
                            WHERE activo = 1 AND telefono LIKE @dato
                            ORDER BY nombres, apellido_paterno";
                    break;
                default:
                    return;
            }

            Bioquimicos_DataTable.Rows.Clear();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand comando = new MySqlCommand(query, connection);
                    comando.Parameters.AddWithValue("@dato", "%" + texto + "%");
                    MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
                    adapter.Fill(Bioquimicos_DataTable);
                }

                if (Bioquimicos_DataTable.Rows.Count == 0)
                {
                    Mensaje = "No se encontraron bioquímicos";
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
                    string deleteQuery = "UPDATE bioquimico SET activo = 0 WHERE idBioquimico = @id";
                    MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@id", id);
                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        TempData["ErrorMessage"] = "Bioquímico no encontrado";
                        return RedirectToPage();
                    }

                    TempData["SuccessMessage"] = "Bioquímico eliminado exitosamente";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}