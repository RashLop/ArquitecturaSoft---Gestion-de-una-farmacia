using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using MedicamentoEntidad = ProyectoArqSoft.Models.Medicamento;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoUpdateModel : BasePageModel
    {
        private readonly IMedicamentoService medicamentoService;

        [BindProperty]
        public int IdMedicamento { get; set; }

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

        public MedicamentoUpdateModel(IMedicamentoService medicamentoService)
        {
            this.medicamentoService = medicamentoService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCargarMedicamentoParaEdicion(int id)
        {
            MedicamentoEntidad? medicamento = medicamentoService.ObtenerPorId(id);

            if (medicamento == null)
                return RedirectToPage("Medicamento", new { error = "Medicamento no encontrado" });

            IdMedicamento = medicamento.Id;
            Nombre = medicamento.Nombre;
            Presentacion = medicamento.Presentacion;
            Clasificacion = medicamento.Clasificacion;
            Concentracion = medicamento.Concentracion;
            Precio = medicamento.Precio;
            Stock = medicamento.Stock;

            return Page();
        }

        public IActionResult OnPostActualizarMedicamento()
        {
            Validacion resultado = medicamentoService.Actualizar(
                IdMedicamento,
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

            return RedirectToPage("Medicamento", new { mensaje = "Medicamento actualizado correctamente" });
        }
    }
}