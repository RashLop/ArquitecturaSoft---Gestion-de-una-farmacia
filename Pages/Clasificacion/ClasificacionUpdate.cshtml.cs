using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
//using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Services;
//using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Pages.Base;

using ClasificacionEntidad = ProyectoArqSoft.Domain.Models.Clasificacion; 

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

        [BindProperty]
        [Display(Name = "Origen")]
        public string Origen { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

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
            Origen = clasificacion.Origen;
            Descripcion = clasificacion.Descripcion;

            return Page();
        }

        public IActionResult OnPostActualizarClasificacion()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                Estado.MensajeError = "No se pudo identificar el usuario que realiza la operación.";
                return Page();
            }

            Result resultado = clasificacionService.Actualizar(IdClasificacion, Nombre, Origen, Descripcion, idUsuario.Value);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Clasificacion", new { mensaje = "Clasificación actualizada correctamente" });
        }
    }
}