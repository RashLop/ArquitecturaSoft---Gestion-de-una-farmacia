using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Domain.Validators;

namespace ProyectoArqSoft.Pages.Bioquimico
{
    public class BioquimicoCreateModel : BasePageModel
    {
        private readonly IUsuarioService usuarioService;

        [BindProperty] public string Nombres { get; set; } = string.Empty;
        [BindProperty] public string ApellidoPaterno { get; set; } = string.Empty;
        [BindProperty] public string ApellidoMaterno { get; set; } = string.Empty;
        [BindProperty] public string Ci { get; set; } = string.Empty;
        [BindProperty] public string CiExtencion { get; set; } = string.Empty;
        [BindProperty] public string Telefono { get; set; } = string.Empty;
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string UserName { get; set; } = string.Empty;
        [BindProperty] public string Password { get; set; } = string.Empty;

        public BioquimicoCreateModel(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        public IActionResult OnGet()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            return Page();
        }

        public IActionResult OnPostCrearBioquimico()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            UsuarioRegistroDto dto = new UsuarioRegistroDto
            {
                Nombres = Nombres,
                ApellidoPaterno = ApellidoPaterno,
                ApellidoMaterno = ApellidoMaterno,
                Ci = Ci,
                CiExtencion = CiExtencion,
                Telefono = Telefono,
                Email = Email,
                UserName = UserName,
                Password = Password
            };

            Result resultado = usuarioService.CrearUsuario(dto, "Bioquimico");

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Bioquimico", new { mensaje = "Bioquímico registrado correctamente" });
        }
    }
}
