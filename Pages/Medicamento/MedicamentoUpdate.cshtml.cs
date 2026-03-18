using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.FactoryCreators;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Validaciones;
using MedicamentoEntidad = ProyectoArqSoft.Models.Medicamento;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoUpdateModel : BasePageModel
    {
        private readonly IRepository<MedicamentoEntidad> repository;
        private readonly IValidacion<MedicamentoEntidad> validador;

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

        public MedicamentoUpdateModel(IConfiguration configuration)
        {
            RepositoryCreator<MedicamentoEntidad> creator = new MedicamentoRepositoryCreator(configuration);
            repository = creator.CreateRepo();
            validador = new MedicamentoValidacion();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCargarMedicamentoParaEdicion(int id)
        {
            MedicamentoEntidad? medicamento = repository.GetById(id);

            if (medicamento == null)
                return RedirectToPage("Medicamento");

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
            MedicamentoEntidad medicamento = ConstruirMedicamento();
            Validacion resultado = ValidarMedicamento(medicamento);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            repository.Update(medicamento);

            return RedirectToPage("Medicamento");
        }

        private MedicamentoEntidad ConstruirMedicamento()
        {
            return new MedicamentoEntidad
            {
                Id = IdMedicamento,
                Nombre = StringHelper.QuitarEspacios(Nombre),
                Presentacion = StringHelper.LimpiarEspacios(Presentacion),
                Clasificacion = StringHelper.LimpiarEspacios(Clasificacion),
                Concentracion = StringHelper.LimpiarEspacios(Concentracion),
                Precio = Precio,
                Stock = Stock
            };
        }

        private Validacion ValidarMedicamento(MedicamentoEntidad medicamento)
        {
            return validador.Validar(medicamento);
        }
    }
}