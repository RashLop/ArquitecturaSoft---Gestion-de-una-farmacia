using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Models;
using System.ComponentModel.DataAnnotations;
using BioquimicoEntidad = ProyectoArqSoft.Models.Bioquimico;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoCreateModel : BasePageModel
    {
        private readonly IBioquimicoService _bioquimicoService;

        [BindProperty]
        public string Nombres { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Carnet de Identidad")]
        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Expedido")]
        public string CiExtencion { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        public BioquimicoCreateModel(IBioquimicoService bioquimicoService)
        {
            _bioquimicoService = bioquimicoService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCrearBioquimico()
        {
            var nuevoBioquimico = new BioquimicoEntidad
            {
                Nombres = Nombres,
                ApellidoPaterno = ApellidoPaterno,
                ApellidoMaterno = ApellidoMaterno,
                Ci = Ci,
                CiExtencion = CiExtencion,
                Telefono = Telefono
            };

            Validacion resultado = _bioquimicoService.Crear(nuevoBioquimico);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Bioquimico");
        }
    }
}