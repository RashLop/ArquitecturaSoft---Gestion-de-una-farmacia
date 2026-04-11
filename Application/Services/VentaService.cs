using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Infrastructure.Helpers;
using ProyectoArqSoft.Domain.Validators;
using System.Data;

namespace ProyectoArqSoft.Application.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _repository;
        private readonly IResult<Venta> _validador;

        public VentaService(
            IVentaRepository repository,
            IResult<Venta> validador)
        {
            _repository = repository;
            _validador = validador;
        }

        public DataTable ObtenerTodos()
        {
            return _repository.GetAll();
        }

        public DataTable ObtenerTodos(string filtro)
        {
            return _repository.GetAll(filtro);
        }

        public Venta? ObtenerPorId(int id)
        {
            return _repository.GetById(id);
        }

        public List<DetalleVenta> ObtenerDetallesPorVenta(int idVenta)
        {
            return _repository.GetDetallesByVentaId(idVenta);
        }

        public Result Crear(
            int idCliente,
            int idUsuario,
            string metodoPago,
            List<DetalleVentaInputDto> detallesInput)
        {
            Venta venta = ConstruirVenta(
                id: 0,
                idCliente: idCliente,
                idUsuario: idUsuario,
                metodoPago: metodoPago,
                detallesInput: detallesInput,
                idUsuarioEditor: null);

            Result validacion = _validador.Validar(venta);
            if (validacion.IsFailure)
                return validacion;

            return _repository.RegistrarVenta(venta);
        }

        public Result Actualizar(
            int idVenta,
            int idCliente,
            string metodoPago,
            List<DetalleVentaInputDto> detallesInput,
            int idUsuarioEditor)
        {
            Venta ventaExistente = _repository.GetById(idVenta) ?? new Venta();

            if (ventaExistente.Id == 0)
                return Result.Fail("La venta no existe.");

            if (ventaExistente.Estado == 0)
                return Result.Fail("No se puede modificar una venta anulada.");

            Venta venta = ConstruirVenta(
                id: idVenta,
                idCliente: idCliente,
                idUsuario: ventaExistente.IdUsuario,
                metodoPago: metodoPago,
                detallesInput: detallesInput,
                idUsuarioEditor: idUsuarioEditor);

            Result validacion = _validador.Validar(venta);
            if (validacion.IsFailure)
                return validacion;

            return _repository.ActualizarVenta(venta);
        }

        public Result EliminarLogicamente(int idVenta, int idUsuarioEditor)
        {
            return _repository.AnularVentaLogicamente(idVenta, idUsuarioEditor);
        }

        private Venta ConstruirVenta(
            int id,
            int idCliente,
            int idUsuario,
            string metodoPago,
            List<DetalleVentaInputDto> detallesInput,
            int? idUsuarioEditor)
        {
            Venta venta = new Venta
            {
                Id = id,
                IdCliente = idCliente,
                IdUsuario = idUsuario,
                IdUsuarioEditor = idUsuarioEditor,
                MetodoPago = StringHelper.LimpiarEspacios(metodoPago),
                Detalles = new List<DetalleVenta>()
            };

            foreach (DetalleVentaInputDto item in detallesInput)
            {
                DetalleVenta detalle = new DetalleVenta
                {
                    IdMedicamento = item.IdMedicamento,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    Subtotal = item.Cantidad * item.PrecioUnitario
                };

                venta.Detalles.Add(detalle);
            }

            venta.Total = venta.Detalles.Sum(x => x.Subtotal);

            return venta;
        }
    }
}