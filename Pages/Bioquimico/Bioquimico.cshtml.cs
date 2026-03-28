using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoModel : BasePageModel
    {
        private readonly IBioquimicoService _bioquimicoService;
        private readonly IValidacion<string> _busquedaValidator;

        public BioquimicoModel(
            IBioquimicoService bioquimicoService,
            IValidacion<string> busquedaValidator)
        {
            _bioquimicoService = bioquimicoService;
            _busquedaValidator = busquedaValidator;
        }

        public DataTable dtBioquimicos { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Filtro { get; set; }

        public void OnGet()
        {
            string filtro = Filtro ?? string.Empty;

            var validacion = _busquedaValidator.Validar(filtro);

            if (validacion.IsFailure)
            {
                Estado.MensajeError = validacion.Error;
                dtBioquimicos = new DataTable();
                return;
            }

            dtBioquimicos = _bioquimicoService.ObtenerTodos(filtro);

            if (dtBioquimicos.Rows.Count == 0 && !string.IsNullOrWhiteSpace(filtro))
            {
                Estado.Mensaje = $"No se encontraron resultados para: {filtro}";
            }
        }

        public IActionResult OnPostEliminar(int id)
        {
            var resultado = _bioquimicoService.Eliminar(id);

            if (resultado.IsSuccess)
            {
                TempData["Mensaje"] = "Bioquímico eliminado correctamente.";
            }
            else
            {
                TempData["Error"] = resultado.Error;
            }

            return RedirectToPage();
        }
    }
}