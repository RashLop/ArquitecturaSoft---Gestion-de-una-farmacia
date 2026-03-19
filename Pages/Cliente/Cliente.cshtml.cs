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
            _logger.LogInformation("=== Página de Clientes cargada ===");
            _logger.LogInformation("Filtro recibido: {Filtro}", filtro);
            _logger.LogInformation("Mensaje: {Mensaje}", mensaje);
            _logger.LogInformation("Error: {Error}", error);

            CargarParametros(filtro, mensaje, error);

            Validacion resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.MensajeError;

            if (!resultado.EsValido)
            {
                _logger.LogWarning("Filtro inválido: {MensajeError}", resultado.MensajeError);
                return;
            }

            _logger.LogInformation("Filtro válido, cargando clientes...");
            CargarClientes(Estado.FiltroActual);

            _logger.LogInformation("Clientes cargados: {Count} filas", ClienteDataTable?.Rows.Count ?? 0);
        }

        public IActionResult OnPostEliminarClienteLogicamente(int id)
        {
            _logger.LogInformation("Eliminando cliente ID: {Id}", id);

            Validacion resultado = _clienteService.EliminarLogicamente(id);

            if (!resultado.EsValido)
            {
                _logger.LogError("Error al eliminar cliente {Id}: {Error}", id, resultado.MensajeError);
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            _logger.LogInformation("Cliente {Id} eliminado exitosamente", id);
            TempData["SuccessMessage"] = "Cliente eliminado correctamente";
            return RedirectToPage();
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro) ?? string.Empty;
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;

            _logger.LogDebug("Parámetros cargados - Filtro: {Filtro}, Mensaje: {Mensaje}, Error: {Error}",
                Estado.FiltroActual, Estado.Mensaje, Estado.MensajeError);
        }

        private void CargarClientes(string filtro)
        {
            try
            {
                _logger.LogInformation("Obteniendo clientes con filtro: '{Filtro}'", filtro);

                ClienteDataTable = _clienteService.ObtenerTodos(filtro);

                _logger.LogInformation("ClienteDataTable obtenido. Null: {IsNull}, Filas: {RowCount}",
                    ClienteDataTable == null, ClienteDataTable?.Rows.Count ?? 0);

                if (ClienteDataTable != null)
                {
                    _logger.LogInformation("Columnas disponibles: {Columns}",
                        string.Join(", ", ClienteDataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));

                    if (ClienteDataTable.Rows.Count == 0)
                    {
                        _logger.LogWarning("No se encontraron clientes con el filtro: '{Filtro}'", filtro);
                        Estado.Mensaje = "No hay clientes registrados";
                    }
                    else
                    {
                        _logger.LogInformation("Primer cliente: {RazonSocial} - {Nit}",
                            ClienteDataTable.Rows[0]["razon_social"],
                            ClienteDataTable.Rows[0]["nit"]);
                    }
                }
                else
                {
                    _logger.LogError("ClienteDataTable es null después de llamar al servicio");
                    Estado.MensajeError = "Error al obtener datos de clientes";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar clientes con filtro: '{Filtro}'", filtro);
                Estado.MensajeError = $"Error al cargar clientes: {ex.Message}";
            }
        }
    }
}