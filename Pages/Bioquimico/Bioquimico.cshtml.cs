using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Pages.Base;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoModel : BasePageModel
    {
        private readonly IBioquimicoService _bioquimicoService;
        private readonly IValidacion<string> _busquedaValidator;

        // El constructor recibe las dependencias configuradas en Program.cs
        public BioquimicoModel(IBioquimicoService bioquimicoService, IValidacion<string> busquedaValidator)
        {
            _bioquimicoService = bioquimicoService;
            _busquedaValidator = busquedaValidator;
        }

        // Propiedad que se llena para mostrar los datos en la tabla del .cshtml
        public DataTable dtBioquimicos { get; set; } = new DataTable();

        // Captura el valor del input name="filtro" de la URL
        [BindProperty(SupportsGet = true)]
        public string? Filtro { get; set; }

        public void OnGet()
{
    // 1. Validar el filtro antes de procesar la búsqueda
    var validacion = _busquedaValidator.Validar(Filtro ?? "");

    if (!validacion.EsValido)
    {
        // CAMBIO: Usar Estado.MensajeError en lugar de TempData
        Estado.MensajeError = validacion.MensajeError; 
        dtBioquimicos = new DataTable(); 
        return;
    }

    // 2. Si es válido, llamamos al servicio para obtener los datos
    dtBioquimicos = _bioquimicoService.ObtenerTodos(Filtro ?? "");

    // Opcional: Si la búsqueda es válida pero no hay resultados
    if (dtBioquimicos.Rows.Count == 0 && !string.IsNullOrWhiteSpace(Filtro))
    {
        Estado.Mensaje = "No se encontraron resultados para: " + Filtro;
    }
}

        public IActionResult OnPostEliminar(int id)
        {
            // Delegamos la eliminación al servicio
            var resultado = _bioquimicoService.Eliminar(id);

            if (resultado.EsValido)
            {
                TempData["Mensaje"] = "Bioquímico eliminado correctamente.";
            }
            else
            {
                // Usamos .Error según tu clase Validacion
                TempData["Error"] = resultado.MensajeError;
            }

            return RedirectToPage();
        }
    }
}