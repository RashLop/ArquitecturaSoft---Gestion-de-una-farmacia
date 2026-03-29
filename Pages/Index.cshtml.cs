using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.FactoryProducts;
using System.Data;
using ProyectoArqSoft.FactoryCreators;
using ProyectoArqSoft.Repositories;

namespace ProyectoArqSoft.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration configuration;

        public string? Usuario { get; set; }

        public DataTable MedicamentoDataTable { get; set; } = new DataTable();

        //estadisticas
        private readonly MedicamentoRepository _medicamentoRepository;
        private readonly ClienteRepository _clienteRepo;
        private readonly BioquimicoRepository _bioquimicoRepo;

        public int TotalMedicamentos { get; set; }
        public int TotalClientes { get; set; }
        public int TotalBioquimicos { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration, MedicamentoRepository medicamentoRepository, ClienteRepository clienteRepo, BioquimicoRepository bioquimicoRepo)
        {
            _logger = logger;
            this.configuration = configuration;
            _medicamentoRepository = medicamentoRepository;
            _clienteRepo  = clienteRepo;
            _bioquimicoRepo = bioquimicoRepo;
        }

        public void OnGet()
        {
            Usuario = HttpContext.Session.GetString("Usuario");
            TotalMedicamentos = _medicamentoRepository.Count();
            TotalClientes = _clienteRepo.Count();
            TotalBioquimicos = _bioquimicoRepo.Count();
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
