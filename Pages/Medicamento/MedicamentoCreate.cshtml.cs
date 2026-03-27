using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoCreateModel : BasePageModel
    {
        private readonly IMedicamentoService medicamentoService;

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Presentación")]
        public string Presentacion { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Clasificación")]
        public string Clasificacion { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Concentración")]
        public string Concentracion { get; set; } = string.Empty;

        [BindProperty]
        public decimal Precio { get; set; }

        [BindProperty]
        public int Stock { get; set; }

        public MedicamentoCreateModel(IMedicamentoService medicamentoService)
        {
            this.medicamentoService = medicamentoService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCrearMedicamento()
        {
            Validacion resultado = medicamentoService.Crear(
                Nombre,
                Presentacion,
                Clasificacion,
                Concentracion,
                Precio,
                Stock);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Medicamento", new { mensaje = "Medicamento registrado correctamente" });
        }
    }
}