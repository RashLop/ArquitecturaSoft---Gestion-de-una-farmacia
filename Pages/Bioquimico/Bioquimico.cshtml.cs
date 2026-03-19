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
       
        
        public BioquimicoModel(IBioquimicoService bioquimicoService, IValidacion<string> busquedaValidator)
        {
            _bioquimicoService = bioquimicoService;
            _busquedaValidator = busquedaValidator;
        }

        
        public DataTable dtBioquimicos { get; set; } = new DataTable();

        
        [BindProperty(SupportsGet = true)]
        public string? Filtro { get; set; }

        public void OnGet()
{
    
    var validacion = _busquedaValidator.Validar(Filtro ?? "");

    if (!validacion.EsValido)
    {
       
        Estado.MensajeError = validacion.MensajeError; 
        dtBioquimicos = new DataTable(); 
        return;
    }

    
    dtBioquimicos = _bioquimicoService.ObtenerTodos(Filtro ?? "");

   
    if (dtBioquimicos.Rows.Count == 0 && !string.IsNullOrWhiteSpace(Filtro))
    {
        Estado.Mensaje = "No se encontraron resultados para: " + Filtro;
    }
}

        public IActionResult OnPostEliminar(int id)
        {
           
            var resultado = _bioquimicoService.Eliminar(id);

            if (resultado.EsValido)
            {
                TempData["Mensaje"] = "Bioquímico eliminado correctamente.";
            }
            else
            {
                
                TempData["Error"] = resultado.MensajeError;
            }

            return RedirectToPage();
        }
    }
}