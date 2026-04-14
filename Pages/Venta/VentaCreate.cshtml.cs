using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Pages.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ProyectoArqSoft.Domain.Validators;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    [Authorize(Roles = "Admin,Bioquimico")]
    public class VentaCreateModel : BasePageModel
    {
        private readonly IVentaFacade ventaFacade;

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        [Display(Name = "Método de Pago")]
        public string MetodoPago { get; set; } = string.Empty;

        [BindProperty]
        public string DetallesJson { get; set; } = "[]";

        public DataTable ClienteDataTable { get; set; } = new DataTable();
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();

        public VentaCreateModel(IVentaFacade ventaFacade)
        {
            this.ventaFacade = ventaFacade;
        }

        public void OnGet()
        {
            CargarCatalogos();
        }

        public IActionResult OnPostCrearVenta()
        {
            // REQUISITO: Auditoría (usuario_idUsuario)
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 1;

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

            // REQUISITO: Patrón Facade y Transacción Principal
            Result resultado = ventaFacade.CrearVenta(
                IdCliente,
                idUsuario.Value,
                MetodoPago,
                detalles);

            if (resultado.IsSuccess == false)
            {
                Estado.MensajeError = resultado.Error;
                CargarCatalogos();
                return Page();
            }

            // REQUISITO: Generación automática de comprobante (Redirección tras éxito)
            return RedirectToPage("Venta", new { mensaje = "Venta registrada correctamente." });
        }

        private void CargarCatalogos()
        {
            ClienteDataTable = ventaFacade.ObtenerClientes();
            MedicamentoDataTable = ventaFacade.ObtenerMedicamentos();
        }
    }
}