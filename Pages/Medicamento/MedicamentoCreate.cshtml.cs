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
    public class MedicamentoCreateModel : BasePageModel
    {
        private readonly IRepository<MedicamentoEntidad> repository;
        private readonly IValidacion<MedicamentoEntidad> validador;

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

        public MedicamentoCreateModel(IConfiguration configuration)
        {
            RepositoryCreator<MedicamentoEntidad> creator = new MedicamentoRepositoryCreator(configuration);
            repository = creator.CreateRepo();
            validador = new MedicamentoValidacion();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCrearMedicamento()
        {
            MedicamentoEntidad medicamento = ConstruirMedicamento();
            Validacion resultado = ValidarMedicamento(medicamento);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            repository.Insert(medicamento);

            return RedirectToPage("Medicamento");
        }

        private Validacion ValidarMedicamento(MedicamentoEntidad medicamento)
        {
            return validador.Validar(medicamento);
        }

        private MedicamentoEntidad ConstruirMedicamento()
        {
            return new MedicamentoEntidad
            {
                Nombre = StringHelper.QuitarEspacios(Nombre),
                Presentacion = StringHelper.Limpiar(Presentacion),
                Clasificacion = StringHelper.Limpiar(Clasificacion),
                Concentracion = StringHelper.Limpiar(Concentracion),
                Precio = Precio,
                Stock = Stock
            };
        }
    }
}