using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Infrastructure.Helpers;
using ProyectoArqSoft.Pages.Base;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    [Authorize(Roles = "Admin,Bioquimico")]
    public class VentaModel : BasePageModel
    {
        private readonly IVentaService ventaService;

        public DataTable VentaDataTable { get; set; } = new DataTable();

        public VentaModel(IVentaService ventaService)
        {
            this.ventaService = ventaService;
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;

            Result resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.Error;

            if (resultado.IsFailure)
                return;

            VentaDataTable = ventaService.ObtenerTodos(Estado.FiltroActual);
        }

        public IActionResult OnPostEliminarVentaLogicamente(int id)
        {
            int? idUsuarioEditor = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuarioEditor == null)
            {
                Estado.MensajeError = "No se pudo identificar el usuario.";
                VentaDataTable = ventaService.ObtenerTodos(Estado.FiltroActual);
                return Page();
            }

            Result resultado = ventaService.EliminarLogicamente(id, idUsuarioEditor.Value);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                VentaDataTable = ventaService.ObtenerTodos(Estado.FiltroActual);
                return Page();
            }

            return RedirectToPage("Venta", new { mensaje = "Venta anulada correctamente." });
        }
    }
}