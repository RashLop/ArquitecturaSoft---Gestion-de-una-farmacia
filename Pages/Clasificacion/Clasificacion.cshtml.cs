using Microsoft.AspNetCore.Mvc;
//using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class ClasificacionModel : BasePageModel
    {
        private readonly IClasificacionService clasificacionService;

        public DataTable ClasificacionDataTable { get; set; } = new DataTable();

        public ClasificacionModel(IClasificacionService clasificacionService)
        {
            this.clasificacionService = clasificacionService;
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            CargarParametros(filtro, mensaje, error);

            Result resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.Error;

            if (resultado.IsFailure)
                return;

            CargarClasificaciones(Estado.FiltroActual);
        }

        public IActionResult OnPostEliminarClasificacionLogicamente(int id)
        {            
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                Estado.MensajeError = "No se pudo identificar el usuario que realiza la operación.";
                CargarClasificaciones(Estado.FiltroActual);
                return Page();
            }

            Result resultado = clasificacionService.EliminarLogicamente(id, idUsuario.Value);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                CargarClasificaciones(Estado.FiltroActual);
                return Page();
            }

            return RedirectToPage("Clasificacion", new { mensaje = "Clasificación eliminada correctamente" });
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private void CargarClasificaciones(string filtro)
        {
            ClasificacionDataTable = clasificacionService.ObtenerTodos(filtro);
        }
    }
}