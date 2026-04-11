using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Domain.DTOs;
using System.Data;
using ProyectoArqSoft.Application.Services;
using ProyectoArqSoft.Infrastructure.Persistence.Connection;

namespace ProyectoArqSoft.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly EstadisticasService _estadisticasFarmacia;
        private readonly string _connectionString;
        public string? Usuario { get; set; }
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();
        public EstadisticasDTO TotalFarmacia { get; set; } = new EstadisticasDTO();

        public IndexModel(
            ILogger<IndexModel> logger,
            IConfiguration configuration,
            EstadisticasService estadisticasFarmacia)
        {
            _logger = logger;
            _configuration = configuration;
            _estadisticasFarmacia = estadisticasFarmacia;
            _connectionString = ConexionStringSingleton.Instancia.CadenaConexion;
        }

        public void OnGet()
        {
            Usuario = HttpContext.Session.GetString("UserName");
            TotalFarmacia = _estadisticasFarmacia.ObtenerEstadisticas();
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
                                    ON m.id_clasificacion = c.id
                                WHERE m.estado = 1
                                ORDER BY RAND()
                                LIMIT 3";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(MedicamentoDataTable);
            }
        }
    }
}
