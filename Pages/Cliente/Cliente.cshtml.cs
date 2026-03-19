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
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClienteModel> _logger;

        public DataTable ClienteDataTable { get; set; } = new DataTable();

        public ClienteModel(IClienteService clienteService, ILogger<ClienteModel> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            CargarParametros(filtro, mensaje, error);

            Validacion resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.MensajeError;

            if (!resultado.EsValido)
                return;

            CargarClientes(Estado.FiltroActual);
        }

        public IActionResult OnPostEliminarClienteLogicamente(int id)
        {
            Validacion resultado = _clienteService.EliminarLogicamente(id);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            TempData["SuccessMessage"] = "Cliente eliminado correctamente";
            return RedirectToPage();
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro) ?? string.Empty;
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private void CargarClientes(string filtro)
        {
            try
            {
                ClienteDataTable = _clienteService.ObtenerTodos(filtro);

                if (ClienteDataTable == null || ClienteDataTable.Rows.Count == 0)
                {
                    Estado.Mensaje = "No hay clientes registrados";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar clientes");
                Estado.MensajeError = $"Error al cargar clientes: {ex.Message}";
            }
        }
    }
}