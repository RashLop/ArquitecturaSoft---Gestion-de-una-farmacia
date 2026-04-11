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
        private readonly IVentaService ventaService;
        private readonly IClienteService clienteService;
        private readonly IMedicamentoService medicamentoService;

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        [Display(Name = "Método de Pago")]
        public string MetodoPago { get; set; } = string.Empty;

        [BindProperty]
        public string DetallesJson { get; set; } = "[]";

        public DataTable ClienteDataTable { get; set; } = new DataTable();
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();

        public VentaCreateModel(
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

        public IActionResult OnPostCrearVenta()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
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

            Result resultado = ventaService.Crear(
                IdCliente,
                idUsuario.Value,
                MetodoPago,
                detalles);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                CargarCatalogos();
                return Page();
            }

            return RedirectToPage("Venta", new { mensaje = "Venta registrada correctamente." });
        }

        private void CargarCatalogos()
        {
            ClienteDataTable = clienteService.ObtenerTodos();
            MedicamentoDataTable = medicamentoService.ObtenerTodos();
        }
    }
}