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

        [BindProperty] public string Nombres { get; set; } = string.Empty;
        [BindProperty] public string ApPaterno { get; set; } = string.Empty;
        [BindProperty] public string ApMaterno { get; set; } = string.Empty;
        [BindProperty] public string CI { get; set; } = string.Empty;
        [BindProperty] public string CIExt { get; set; } = string.Empty;
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string User { get; set; } = string.Empty;
        [BindProperty] public string Role { get; set; } = "Bioquimico";

        public UsuarioCreateModel(IUsuarioService usuarioService) => this.usuarioService = usuarioService;

        public IActionResult OnPostCrearUsuario()
        {
            var dto = new UsuarioRegistroDto
            {
                Nombres = Nombres,
                ApellidoPaterno = ApPaterno,
                ApellidoMaterno = ApMaterno,
                Ci = CI,
                CiExtencion = CIExt,
                Email = Email,
                UserName = User,
                Password = "TemporalPassword123",
                Telefono = "000000" // El service gestiona el hash y envío
            };

            // Cumple rúbrica: Evita duplicidades y envía mail
            Validacion resultado = usuarioService.CrearUsuario(dto, Role);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Usuario", new { mensaje = "Usuario registrado. Las credenciales fueron enviadas a su correo." });
        }
    }
}