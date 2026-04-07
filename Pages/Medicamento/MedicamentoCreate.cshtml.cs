using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Domain.Validators;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoCreateModel : BasePageModel
    {
        private readonly IMedicamentoService medicamentoService;
        private readonly IClasificacionService clasificacionService;
        public DataTable ClasificacionDataTable { get; set; } = new DataTable();

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Presentación")]
        public string Presentacion { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Clasificación")]
        public int IdClasificacion { get; set; }

        [BindProperty]
        [Display(Name = "Concentración")]
        public string Concentracion { get; set; } = string.Empty;

        [BindProperty]
        public decimal Precio { get; set; }

        [BindProperty]
        public int Stock { get; set; }

        public MedicamentoCreateModel(IMedicamentoService medicamentoService, IClasificacionService clasificacionService)
        {
            this.medicamentoService = medicamentoService;
            this.clasificacionService = clasificacionService;
        }

        public void OnGet()
        {
            CargarClasificaciones();
        }

        public IActionResult OnPostCrearMedicamento()
        {
            Result resultado = medicamentoService.Crear(
                Nombre,
                Presentacion,
                IdClasificacion,
                Concentracion,
                Precio,
                Stock);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                CargarClasificaciones();
                return Page();
            }

            return RedirectToPage("Medicamento", new { mensaje = "Medicamento registrado correctamente" });
        }

        private void CargarClasificaciones()
        {
            ClasificacionDataTable = clasificacionService.ObtenerTodos();
        }
    }
}