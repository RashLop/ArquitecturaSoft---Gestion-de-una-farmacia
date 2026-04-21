using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Application.Services;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Pages.Base;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;

namespace ProyectoArqSoft.Pages
{
    [Authorize(Roles = "Admin,Bioquimico")]
    public class VentaCreateModel : BasePageModel
    {
        private readonly IVentaFacade ventaFacade;
        private readonly IClienteService clienteService;
        private readonly IMedicamentoService medicamentoService;
        private readonly ComprobanteVentaPdfService comprobanteVentaPdfService;

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        [Display(Name = "Método de Pago")]
        public string MetodoPago { get; set; } = string.Empty;

        [BindProperty]
        public string DetallesJson { get; set; } = "[]";

        public DataTable ClienteDataTable { get; set; } = new();
        public DataTable MedicamentoDataTable { get; set; } = new();

        public VentaCreateModel(
            IVentaFacade ventaFacade,
            IClienteService clienteService,
            IMedicamentoService medicamentoService,
            ComprobanteVentaPdfService comprobanteVentaPdfService)
        {
            this.ventaFacade = ventaFacade;
            this.clienteService = clienteService;
            this.medicamentoService = medicamentoService;
            this.comprobanteVentaPdfService = comprobanteVentaPdfService;
        }

        public void OnGet()
        {
            CargarCatalogos();
        }

        public IActionResult OnPostCrearVenta()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            string cajero = HttpContext.Session.GetString("UserName") ?? "cajero";

            if (idUsuario == null)
            {
                Estado.MensajeError = "No se pudo identificar el usuario que registra la venta.";
                CargarCatalogos();
                return Page();
            }

            List<DetalleVentaInputDto> detalles;
            try
            {
                detalles = JsonSerializer.Deserialize<List<DetalleVentaInputDto>>(DetallesJson) ?? new();

                detalles = detalles
                    .Where(x => x.IdMedicamento > 0 && x.Cantidad > 0)
                    .GroupBy(x => x.IdMedicamento)
                    .Select(g => new DetalleVentaInputDto
                    {
                        IdMedicamento = g.Key,
                        Cantidad = g.Sum(x => x.Cantidad)
                    })
                    .ToList();

                DetallesJson = JsonSerializer.Serialize(detalles);
            }
            catch
            {
                Estado.MensajeError = "El detalle de la venta no tiene un formato válido.";
                CargarCatalogos();
                return Page();
            }

            if (IdCliente <= 0)
            {
                Estado.MensajeError = "Debe seleccionar un cliente válido.";
                CargarCatalogos();
                return Page();
            }

            if (string.IsNullOrWhiteSpace(MetodoPago))
            {
                Estado.MensajeError = "Debe seleccionar un método de pago.";
                CargarCatalogos();
                return Page();
            }

            if (!detalles.Any())
            {
                Estado.MensajeError = "Debe agregar al menos un medicamento.";
                CargarCatalogos();
                return Page();
            }

            foreach (var detalle in detalles)
            {
                if (detalle.IdMedicamento <= 0)
                {
                    Estado.MensajeError = "Uno de los medicamentos no fue seleccionado correctamente.";
                    CargarCatalogos();
                    return Page();
                }

                if (detalle.Cantidad <= 0)
                {
                    Estado.MensajeError = "La cantidad debe ser mayor a cero.";
                    CargarCatalogos();
                    return Page();
                }
            }

            Result resultado = ventaFacade.CrearVenta(
                IdCliente,
                idUsuario.Value,
                MetodoPago,
                detalles
            );

            if (!resultado.IsSuccess)
            {
                Estado.MensajeError = resultado.Error;
                CargarCatalogos();
                return Page();
            }

            var cliente = clienteService.ObtenerPorId(IdCliente);
            if (cliente == null)
            {
                Estado.MensajeError = "No se pudo obtener la información del cliente para el comprobante.";
                CargarCatalogos();
                return Page();
            }

            var comprobante = new ComprobanteVentaPdfDto
            {
                Fecha = DateTime.Now,
                Nit = cliente.Nit,
                RazonSocial = cliente.RazonSocial,
                Cajero = cajero
            };

            foreach (var item in detalles)
            {
                var medicamento = medicamentoService.ObtenerPorId(item.IdMedicamento);

                comprobante.Detalles.Add(new ComprobanteVentaDetallePdfDto
                {
                    Cantidad = item.Cantidad,
                    Descripcion = medicamento?.Nombre ?? $"Medicamento #{item.IdMedicamento}",
                    PrecioUnitario = medicamento?.Precio ?? 0
                });
            }

            comprobante.Total = comprobante.Detalles.Sum(x => x.Importe);

byte[] pdf = comprobanteVentaPdfService.Generar(comprobante);

Response.Headers["X-Mensaje-Exito"] = "Venta registrada correctamente.";
Response.Headers["X-Redirect-To"] =
    Url.Page("Venta", new { mensaje = "Venta registrada correctamente." }) ?? "/Venta?mensaje=Venta%20registrada%20correctamente.";

return File(pdf, "application/pdf", $"comprobante-venta-{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }

        private void CargarCatalogos()
        {
            ClienteDataTable = ventaFacade.ObtenerClientes();
            MedicamentoDataTable = ventaFacade.ObtenerMedicamentos();
        }
    }
}