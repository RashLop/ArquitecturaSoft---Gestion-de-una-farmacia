using ProyectoArqSoft.Application.Interfaces;
using System.Data;

namespace ProyectoArqSoft.Application.Facades
{
    public class FachadaActualizarStock
    {
        private readonly IMedicamentoService _medicamentoService;

        public FachadaActualizarStock(
            IMedicamentoService medicamentoService)
        {
            _medicamentoService = medicamentoService;
        }

        public DataTable ObtenerMedicamentos()
        {
            return _medicamentoService.ObtenerTodos();
        }
    }
}
