using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;

        public RegisterModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [BindProperty]
        public UsuarioRegistroDto Registro { get; set; } = new();

        public string MensajeError { get; set; } = string.Empty;
        public string MensajeOk { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            string role = "user";

            Validacion resultado = _usuarioService.CrearUsuario(Registro, role);

            if (!resultado.IsSuccess)
            {
                MensajeError = resultado.Error;
                return Page();
            }

            MensajeOk = "Usuario registrado correctamente.";

            Registro = new UsuarioRegistroDto();

            return Page();
        }
    }
}