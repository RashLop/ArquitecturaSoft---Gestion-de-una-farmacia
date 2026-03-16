using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Application.Services;
using ProyectoArqSoft.Application.Validaciones;
using ProyectoArqSoft.Base;
using ProyectoArqSoft.Helpers;
using MedicamentoEntidad = ProyectoArqSoft.Domain.Entities.Medicamento.Medicamento;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoCreateModel : BasePageModel
    {
        private readonly MedicamentoService service;

        public MedicamentoCreateModel(MedicamentoService service)
        {
            this.service = service;
        }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Presentacion { get; set; } = string.Empty;

        [BindProperty]
        public string Clasificacion { get; set; } = string.Empty;

        [BindProperty]
        public string Concentracion { get; set; } = string.Empty;

        [BindProperty]
        public decimal Precio { get; set; }

        [BindProperty]
        public int Stock { get; set; }

        public IActionResult OnPostCrearMedicamento()
        {
            MedicamentoEntidad medicamento = new MedicamentoEntidad
            {
                Nombre = Nombre,
                Presentacion = Presentacion,
                Clasificacion = Clasificacion,
                Concentracion = Concentracion,
                Precio = Precio,
                Stock = Stock
            };

            service.CrearMedicamento(medicamento);

            return RedirectToPage("Medicamento");
        }
    }
}