using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using BioquimicoEntidad = ProyectoArqSoft.Models.Bioquimico;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoEditModel : BasePageModel
    {
        private readonly IBioquimicoService _bioquimicoService;

        [BindProperty]
        public int IdBioquimico { get; set; }

        [BindProperty]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "CI")]
        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Extensión")]
        public string CiExtencion { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        public BioquimicoEditModel(IBioquimicoService bioquimicoService)
        {
            _bioquimicoService = bioquimicoService;
        }

        public void OnGet()
        {
            // El OnGet queda vacío porque usamos el handler de cargar
        }

        public IActionResult OnPostCargarBioquimicoParaEdicion(int id)
        {
            BioquimicoEntidad? bioquimico = _bioquimicoService.ObtenerPorId(id);

            if (bioquimico == null)
                return RedirectToPage("Bioquimico");

            // Mapeamos la entidad a las propiedades del Model
            IdBioquimico = bioquimico.IdBioquimico;
            Nombres = bioquimico.Nombres;
            ApellidoPaterno = bioquimico.ApellidoPaterno;
            ApellidoMaterno = bioquimico.ApellidoMaterno;
            Ci = bioquimico.Ci;
            CiExtencion = bioquimico.CiExtencion;
            Telefono = bioquimico.Telefono;

            return Page();
        }

        public IActionResult OnPostActualizarBioquimico()
        {
            // Creamos el objeto con los datos del formulario
            var bioquimicoEditado = new BioquimicoEntidad
            {
                IdBioquimico = IdBioquimico,
                Nombres = Nombres,
                ApellidoPaterno = ApellidoPaterno,
                ApellidoMaterno = ApellidoMaterno,
                Ci = Ci,
                CiExtencion = CiExtencion,
                Telefono = Telefono
            };

            // Delegamos la validación y actualización al servicio
            Validacion resultado = _bioquimicoService.Actualizar(bioquimicoEditado);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            // Si todo sale bien, volvemos a la lista principal
            return RedirectToPage("Bioquimico");
        }
    }
}