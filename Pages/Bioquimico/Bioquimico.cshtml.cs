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
                // Si el filtro es inválido (ej: un punto "."), enviamos el error a la vista
                TempData["Error"] = validacion.MensajeError;
                dtBioquimicos = new DataTable(); // Aseguramos que la tabla esté vacía
                return;
            }

            // 2. Si es válido, llamamos al servicio para obtener los datos
            dtBioquimicos = _bioquimicoService.ObtenerTodos(Filtro ?? "");
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