using System.Data;

namespace ProyectoArqSoft.Domain.DTOs
{
    public class DashboardDTO
    {
        public EstadisticasDTO Estadisticas { get; set; } = new();
        public DataTable MedicamentosDestacados { get; set; } = new();
    }
}
