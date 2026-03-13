using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration configuration;

        public string? Usuario { get; set; }

        public DataTable MedicamentoDataTable { get; set; } = new DataTable();

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        public void OnGet()
        {
            Usuario = HttpContext.Session.GetString("Usuario");

            CargarMedicamentosDestacados();
        }

        private void CargarMedicamentosDestacados()
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT nombre,
                                        presentacion,
                                        clasificacion,
                                        concentracion,
                                        precio
                                 FROM medicamento
                                 WHERE estado = 1
                                 ORDER BY RAND()
                                 LIMIT 3";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(MedicamentoDataTable);
            }
        }
    }
}
