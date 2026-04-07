using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.Validators;
using MedicamentoEntidad = ProyectoArqSoft.Domain.Models.Medicamento;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoArqSoft.Pages
{
    [Authorize(Roles = "Admin, Bioquimico")]
    public class MedicamentoUpdateModel : BasePageModel
    {
        private readonly IMedicamentoService medicamentoService;
        private readonly IClasificacionService clasificacionService;
        public DataTable ClasificacionDataTable { get; set; } = new DataTable();

        [BindProperty]
        public int IdMedicamento { get; set; }

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

        public MedicamentoUpdateModel(IMedicamentoService medicamentoService, IClasificacionService clasificacionService)
        {
            this.medicamentoService = medicamentoService;
            this.clasificacionService = clasificacionService;
        }

        public void OnGet()
        {
            CargarClasificaciones();
        }

        public IActionResult OnPostCargarMedicamentoParaEdicion(int id)
        {
            MedicamentoEntidad? medicamento = medicamentoService.ObtenerPorId(id);

            if (medicamento == null)
                return RedirectToPage("Medicamento", new { error = "Medicamento no encontrado" });

            IdMedicamento = medicamento.Id;
            Nombre = medicamento.Nombre;
            Presentacion = medicamento.Presentacion;
            IdClasificacion = medicamento.IdClasificacion;
            Concentracion = medicamento.Concentracion;
            Precio = medicamento.Precio;
            Stock = medicamento.Stock;

            CargarClasificaciones();
            return Page();
        }

        public IActionResult OnPostActualizarMedicamento()
        {
            Result resultado = medicamentoService.Actualizar(
                IdMedicamento,
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

            return RedirectToPage("Medicamento", new { mensaje = "Medicamento actualizado correctamente" });
        }

        private void CargarClasificaciones()
        {
            ClasificacionDataTable = clasificacionService.ObtenerTodos();
        }
    }
}