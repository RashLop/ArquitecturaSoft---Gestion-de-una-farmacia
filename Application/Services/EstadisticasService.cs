using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Infrastructure.Persistence.Repositories;

namespace ProyectoArqSoft.Application.Services
{
    public class EstadisticasService
    {
        private readonly MedicamentoRepository _medicamentoRepo;
        private readonly ClienteRepository _clienteRepo;
        private readonly UsuarioRepository _usuarioRepo;

        public EstadisticasService(
            MedicamentoRepository medicamentoRepo,
            ClienteRepository clienteRepo,
            UsuarioRepository usuarioRepo
            )
        {
            _medicamentoRepo = medicamentoRepo;
            _clienteRepo = clienteRepo;
            _usuarioRepo = usuarioRepo;
        }

        public EstadisticasDTO ObtenerEstadisticas()
        {
            return new EstadisticasDTO
            {
                TotalMedicamentos = _medicamentoRepo.Count(),
                TotalClientes = _clienteRepo.Count(),
                TotalUsuarios = _usuarioRepo.Count(),
            };
        }
    }
}
