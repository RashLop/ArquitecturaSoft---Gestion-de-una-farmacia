using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoModel : BasePageModel
    {
        private readonly IMedicamentoService medicamentoService;

        public DataTable MedicamentoDataTable { get; set; } = new DataTable();

        public MedicamentoModel(IMedicamentoService medicamentoService)
        {
            this.medicamentoService = medicamentoService;
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            CargarParametros(filtro, mensaje, error);

            Validacion resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.Error;

            if (resultado.IsFailure)
                return;

            CargarMedicamentos(Estado.FiltroActual);
        }

        public IActionResult OnPostEliminarMedicamentoLogicamente(int id)
        {
            Validacion resultado = medicamentoService.EliminarLogicamente(id);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage();
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private void CargarMedicamentos(string filtro)
        {
            MedicamentoDataTable = medicamentoService.ObtenerTodos(filtro);
        }
    }
}