using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class ClienteModel : BasePageModel
    {
        private readonly IClienteService clienteService;

        public DataTable ClienteDataTable { get; set; } = new DataTable();

        public ClienteModel(IClienteService clienteService)
        {
            this.clienteService = clienteService;
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            CargarParametros(filtro, mensaje, error);

            Validacion resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.Error;

            if (resultado.IsFailure)
                return;

            CargarClientes(Estado.FiltroActual);
        }

        public IActionResult OnPostEliminarClienteLogicamente(int id)
        {
            Validacion resultado = clienteService.Eliminar(id);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Cliente", new { mensaje = "Cliente eliminado correctamente" });
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private void CargarClientes(string filtro)
        {
            ClienteDataTable = clienteService.ObtenerTodos(filtro);
        }
    }
}