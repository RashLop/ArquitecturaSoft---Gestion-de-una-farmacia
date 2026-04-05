using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages.Usuarios
{
    public class UsuarioCreateModel : BasePageModel
    {
        private readonly IUsuarioService usuarioService;

        [BindProperty] public string nombres { get; set; } = string.Empty;
        [BindProperty] public string apPaterno { get; set; } = string.Empty;
        [BindProperty] public string apMaterno { get; set; } = string.Empty;
        [BindProperty] public string ci { get; set; } = string.Empty;
        [BindProperty] public string ciExtencion { get; set; } = string.Empty;
        [BindProperty] public string telefono { get; set; } = string.Empty;
        [BindProperty] public string email { get; set; } = string.Empty;
        [BindProperty] public string user_name { get; set; } = string.Empty;
        [BindProperty] public string pass { get; set; } = string.Empty;
        [BindProperty] public string role { get; set; } = "Bioquimico";

        public UsuarioCreateModel(IUsuarioService usuarioService) => this.usuarioService = usuarioService;

        public IActionResult OnPostCrearUsuario()
        {
            var dto = new UsuarioRegistroDto
            {
                Nombres = nombres,
                ApellidoPaterno = apPaterno,
                ApellidoMaterno = apMaterno,
                Ci = ci,
                CiExtencion = ciExtencion,
                Telefono = telefono,
                Email = email,
                UserName = user_name,
                Password = pass
            };

            Result resultado = usuarioService.CrearUsuario(dto, role);
            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }
            return RedirectToPage("Usuario", new { mensaje = "Usuario registrado correctamente" });
        }
    }
}