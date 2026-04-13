using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.DTOs;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDashboardFacade _dashboardFacade;

        public string? Usuario { get; set; }
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();
        public EstadisticasDTO TotalFarmacia { get; set; } = new EstadisticasDTO();

        public IndexModel(
            ILogger<IndexModel> logger,
            IConfiguration configuration,
            IDashboardFacade dashboardFacade)
        {
            _logger = logger;
            _configuration = configuration;
            _dashboardFacade = dashboardFacade;
        }

        public void OnGet()
        {
            Usuario = HttpContext.Session.GetString("UserName");

            DashboardDTO dashboard = _dashboardFacade.ObtenerDashboardCompleto();

            TotalFarmacia = dashboard.Estadisticas;

            MedicamentoDataTable = dashboard.MedicamentosDestacados;
        }
    }
}