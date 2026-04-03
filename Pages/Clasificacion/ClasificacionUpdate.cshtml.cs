using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
//using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Services;
//using ProyectoArqSoft.Domain.Model.Clasificacion;
using ProyectoArqSoft.Models;
//using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Pages.Base;

using ClasificacionEntidad = ProyectoArqSoft.Models.Clasificacion; // cambiar namespace de Clasificacion de namespace ProyectoArqSoft.Models a namespace ProyectoArqSoft.Domain.Model

namespace ProyectoArqSoft.Pages
{
    public class ClasificacionUpdateModel : BasePageModel
    {
        private readonly IClasificacionService clasificacionService;

        [BindProperty]
        public int IdClasificacion { get; set; }

        [BindProperty]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        public ClasificacionUpdateModel(IClasificacionService clasificacionService)
        {
            this.clasificacionService = clasificacionService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCargarClasificacionParaEdicion(int id)
        {
            ClasificacionEntidad? clasificacion = clasificacionService.ObtenerPorId(id);

            if (clasificacion == null)
                return RedirectToPage("Clasificacion", new { error = "Clasificación no encontrada" });

            IdClasificacion = clasificacion.Id;
            Nombre = clasificacion.Nombre;

            return Page();
        }

        public IActionResult OnPostActualizarClasificacion()
        {
            Validacion resultado = clasificacionService.Actualizar(IdClasificacion, Nombre);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Clasificacion", new { mensaje = "Clasificación actualizada correctamente" });
        }
    }
}