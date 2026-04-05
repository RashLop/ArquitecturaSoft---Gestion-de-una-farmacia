using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
//using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Services;
//using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Pages.Base;

namespace ProyectoArqSoft.Pages
{
    public class ClasificacionCreateModel : BasePageModel
    {
        private readonly IClasificacionService clasificacionService;

        [BindProperty]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Origen")]
        public string Origen { get; set; } = string.Empty;

        public ClasificacionCreateModel(IClasificacionService clasificacionService)
        {
            this.clasificacionService = clasificacionService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCrearClasificacion()
        {
            Result resultado = clasificacionService.Crear(Nombre, Origen);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Clasificacion", new { mensaje = "Clasificación registrada correctamente" });
        }
    }
}