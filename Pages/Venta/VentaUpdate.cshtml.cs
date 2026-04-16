using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Pages.Base;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using DetalleVenta = ProyectoArqSoft.Domain.Models.DetalleVenta;
using VentaEntidad = ProyectoArqSoft.Domain.Models.Venta;

namespace ProyectoArqSoft.Pages
{
    [Authorize(Roles = "Admin,Bioquimico")]
    public class VentaUpdateModel : BasePageModel
    {
        private readonly IVentaFacade ventaFacade;

        [BindProperty]
        public int IdVenta { get; set; }

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        [Display(Name = "MÃ©todo de Pago")]
        public string MetodoPago { get; set; } = string.Empty;

        [BindProperty]
        public string DetallesJson { get; set; } = "[]";

        public DataTable ClienteDataTable { get; set; } = new DataTable();
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();

        public VentaUpdateModel(IVentaFacade ventaFacade)
        {
            this.ventaFacade = ventaFacade;
        }

        public void OnGet()
        {
            CargarCatalogos();
        }

        public IActionResult OnPostCargarVenta(int id)
        {
            VentaEntidad? venta = ventaFacade.ObtenerVentaPorId(id);

            if (venta == null)
                return RedirectToPage("Venta", new { error = "Venta no encontrada." });

            List<DetalleVenta> detalles = ventaFacade.ObtenerDetalles(id);

            IdVenta = venta.Id;
            IdCliente = venta.IdCliente;
            MetodoPago = venta.MetodoPago;

            List<DetalleVentaDto> detallesInput = detalles.Select(x => new DetalleVentaDto
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
            return RedirectToPage("Venta", new { mensaje = "Venta actualizada correctamente." });
        }

        private void CargarCatalogos()
        {
            ClienteDataTable = ventaFacade.ObtenerClientes();
            MedicamentoDataTable = ventaFacade.ObtenerMedicamentos();
        }
    }
}
