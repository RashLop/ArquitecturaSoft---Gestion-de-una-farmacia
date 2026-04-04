using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages.Usuarios
{
    public class UsuarioUpdateModel : BasePageModel
    {
        private readonly IUsuarioService usuarioService;

        [BindProperty] public int IdUsuario { get; set; }
        [BindProperty] public string Nombres { get; set; } = string.Empty;
        [BindProperty] public string ApellidoPaterno { get; set; } = string.Empty;
        [BindProperty] public string ApellidoMaterno { get; set; } = string.Empty;
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string Telefono { get; set; } = string.Empty;
        [BindProperty] public string UserName { get; set; } = string.Empty;
        [BindProperty] public string Role { get; set; } = string.Empty;
        [BindProperty] public sbyte Activo { get; set; }

        public UsuarioUpdateModel(IUsuarioService usuarioService) => this.usuarioService = usuarioService;

        public IActionResult OnPostCargarUsuarioParaEdicion(int id)
        {
            var user = usuarioService.ObtenerUsuarioPorId(id);
            if (user == null) return RedirectToPage("Usuario", new { error = "Usuario no encontrado" });

            IdUsuario = user.IdUsuario;
            UserName = user.UserName;
            Email = user.Email;
            Role = user.Role;
            // Nota: Aquí rellena los campos adicionales que tu ObtenerUsuarioPorId devuelva
            return Page();
        }

        public IActionResult OnPostActualizarUsuario()
        {
            var dto = new UsuarioActualizacionDto
            {
                IdUsuario = IdUsuario,
                Nombres = Nombres,
                ApellidoPaterno = ApellidoPaterno,
                ApellidoMaterno = ApellidoMaterno,
                Email = Email,
                Telefono = Telefono,
                UserName = UserName,
                Role = Role,
                Activo = Activo,
                MustChangePassword = 0
            };

            Validacion resultado = usuarioService.ActualizarUsuario(dto);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Usuario", new { mensaje = "Datos actualizados correctamente" });
        }
    }
}