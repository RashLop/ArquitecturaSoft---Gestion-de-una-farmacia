using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.DTOs;
using VentaEntidad = ProyectoArqSoft.Domain.Models.Venta;
using DetalleVenta = ProyectoArqSoft.Domain.Models.DetalleVenta;
using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Pages.Base;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;

namespace ProyectoArqSoft.Pages
{
    [Authorize(Roles = "Admin,Bioquimico")]
    public class VentaUpdateModel : BasePageModel
    {
        private readonly IVentaService ventaService;
        private readonly IClienteService clienteService;
        private readonly IMedicamentoService medicamentoService;

        [BindProperty]
        public int IdVenta { get; set; }

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        [Display(Name = "Método de Pago")]
        public string MetodoPago { get; set; } = string.Empty;

        [BindProperty]
        public string DetallesJson { get; set; } = "[]";

        public DataTable ClienteDataTable { get; set; } = new DataTable();
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();

        public VentaUpdateModel(
            IVentaService ventaService,
            IClienteService clienteService,
            IMedicamentoService medicamentoService)
        {
            this.ventaService = ventaService;
            this.clienteService = clienteService;
            this.medicamentoService = medicamentoService;
        }

        public void OnGet()
        {
            CargarCatalogos();
        }

        public IActionResult OnPostCargarVenta(int id)
        {
            VentaEntidad? venta = ventaService.ObtenerPorId(id);

            if (venta == null)
                return RedirectToPage("Venta", new { error = "Venta no encontrada." });

            if (venta.Estado == 0)
                return RedirectToPage("Venta", new { error = "No se puede editar una venta anulada." });

            List<DetalleVenta> detalles = ventaService.ObtenerDetallesPorVenta(id);

            IdVenta = venta.Id;
            IdCliente = venta.IdCliente;
            MetodoPago = venta.MetodoPago;

            List<DetalleVentaInputDto> detallesInput = detalles.Select(x => new DetalleVentaInputDto
            {
                IdMedicamento = x.IdMedicamento,
                Cantidad = x.Cantidad,
                PrecioUnitario = x.PrecioUnitario
            }).ToList();

            DetallesJson = JsonSerializer.Serialize(detallesInput);

            CargarCatalogos();
            return Page();
        }

        public IActionResult OnPostActualizarVenta()
        {
            int? idUsuarioEditor = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuarioEditor == null)
            {
                Estado.MensajeError = "No se pudo identificar el usuario.";
                CargarCatalogos();
                return Page();
            }

            List<DetalleVentaInputDto> detalles;
            try
            {
                detalles = JsonSerializer.Deserialize<List<DetalleVentaInputDto>>(DetallesJson) ?? new();
            }
            catch
            {
                Estado.MensajeError = "El detalle de la venta no tiene un formato válido.";
                CargarCatalogos();
                return Page();
            }

            Result resultado = ventaService.Actualizar(
                IdVenta,
                IdCliente,
                MetodoPago,
                detalles,
                idUsuarioEditor.Value);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                CargarCatalogos();
                return Page();
            }

            return RedirectToPage("Venta", new { mensaje = "Venta actualizada correctamente." });
        }

        private void CargarCatalogos()
        {
            ClienteDataTable = clienteService.ObtenerTodos();
            MedicamentoDataTable = medicamentoService.ObtenerTodos();
        }
    }
}