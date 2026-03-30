using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Repositories;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly MedicamentoRepository _medicamentoRepository;

        public string? Usuario { get; set; }
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();
        public int TotalMedicamentos { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IConfiguration configuration,
            MedicamentoRepository medicamentoRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _medicamentoRepository = medicamentoRepository;
        }

        public void OnGet()
        {
            Usuario = HttpContext.Session.GetString("UserName");
            TotalMedicamentos = _medicamentoRepository.Count();
            CargarMedicamentosDestacados();
        }

        private void CargarMedicamentosDestacados()
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection")!;

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