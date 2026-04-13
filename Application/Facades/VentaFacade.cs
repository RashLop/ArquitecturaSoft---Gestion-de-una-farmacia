using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Domain.Validators;
using System.Data;

namespace ProyectoArqSoft.Application.Facades
{
    public class VentaFacade : IVentaFacade
    {
        private readonly IVentaService _ventaService;
        private readonly IClienteService _clienteService;
        private readonly IMedicamentoService _medicamentoService;

        public VentaFacade(
            IVentaService ventaService,
            IClienteService clienteService,
            IMedicamentoService medicamentoService)
        {
            _ventaService = ventaService;
            _clienteService = clienteService;
            _medicamentoService = medicamentoService;
        }

        public DataTable ObtenerVentas(string filtro)
            => _ventaService.ObtenerTodos(filtro);

        public Venta? ObtenerVentaPorId(int id)
            => _ventaService.ObtenerPorId(id);

        public List<DetalleVenta> ObtenerDetalles(int idVenta)
            => _ventaService.ObtenerDetallesPorVenta(idVenta);

        public Result CrearVenta(
            int idCliente,
            int idUsuario,
            string metodoPago,
            List<DetalleVentaInputDto> detalles)
            => _ventaService.Crear(idCliente, idUsuario, metodoPago, detalles);

        public Result ActualizarVenta(
            int idVenta,
            int idCliente,
            string metodoPago,
            List<DetalleVentaInputDto> detalles,
            int idUsuarioEditor)
            => _ventaService.Actualizar(
                idVenta,
                idCliente,
                metodoPago,
                detalles,
                idUsuarioEditor);

        public Result AnularVenta(int idVenta, int idUsuarioEditor)
            => _ventaService.EliminarLogicamente(idVenta, idUsuarioEditor);

        public DataTable ObtenerClientes()
            => _clienteService.ObtenerTodos();

        public DataTable ObtenerMedicamentos()
            => _medicamentoService.ObtenerTodos();
    }
}
    