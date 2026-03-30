using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Repositories;
using System.Data;
using ProyectoArqSoft.Services;

namespace ProyectoArqSoft.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly MedicamentoRepository _medicamentoRepository;
        private readonly string _connectionString;
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
            _connectionString = ConexionStringSingleton.Instancia.CadenaConexion;
        }

        public void OnGet()
        {
            Usuario = HttpContext.Session.GetString("UserName");
            TotalMedicamentos = _medicamentoRepository.Count();
            CargarMedicamentosDestacados();
        }

        private void CargarMedicamentosDestacados()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT m.nombre,
                                        m.presentacion,
                                        c.nombre AS clasificacion,
                                        m.concentracion,
                                        m.precio
                                FROM medicamento m
                                INNER JOIN clasificacion c 
                                    ON m.id_clasificacion = c.id_clasificacion
                                WHERE m.estado = 1
                                ORDER BY RAND()
                                LIMIT 3";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(MedicamentoDataTable);
            }
        }
    }
}