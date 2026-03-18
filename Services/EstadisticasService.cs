using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Model.DTOS;
using ProyectoArqSoft.Pages;


namespace ProyectoArqSoft.Services
{
    public class EstadisticasService
    {
        private readonly MedicamentoRepository _medicamentoRepo;
        //private readonly ClienteRepository _clienteRepo;
        //private readonly BioquimicoRepository _bioquimicoRepo;

        public EstadisticasService(
            MedicamentoRepository medicamentoRepo
            //ClienteRepository clienteRepo,
            //BioquimicoRepository bioquimicoRepo
            )
        {
            _medicamentoRepo = medicamentoRepo;
            //_clienteRepo = clienteRepo;
            //_bioquimicoRepo = bioquimicoRepo;
        }

        public EstadisticasDTO ObtenerEstadisticas()
        {
            return new EstadisticasDTO
            {
                TotalMedicamentos = _medicamentoRepo.Count()
                //TotalClientes = _clienteRepo.Count(),
                //TotalBioquimicos = _bioquimicoRepo.Count(),
            };
        }
    }
}
