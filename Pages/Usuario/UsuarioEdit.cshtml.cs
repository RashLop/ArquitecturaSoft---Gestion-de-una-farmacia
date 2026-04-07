using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Domain.DTOs;

namespace ProyectoArqSoft.Pages.Usuario
{
    public class UsuarioEditModel : BasePageModel
    {
        private readonly IUsuarioService _usuarioService;

        [BindProperty]
        public UsuarioEdicionDto Input { get; set; } = new();

        public UsuarioEditModel(IUsuarioService usuarioService) => _usuarioService = usuarioService;

        public IActionResult OnPostCargarUsuarioParaEdicion(int id)
        {
            var user = _usuarioService.ObtenerUsuarioPorId(id);
            if (user == null) return RedirectToPage("Usuario", new { error = "Usuario no encontrado" });

            Input.IdUsuario = user.IdUsuario;
            Input.Email = user.Email;
            Input.Role = user.Role;
            Input.Activo = user.Activo;
            return Page();
        }

        public IActionResult OnPostActualizarUsuario()
        {
            int? idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario");

            Result resultado = _usuarioService.ActualizarUsuarioEdicion(Input, idUsuarioSesion);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Usuario", new { mensaje = "Perfil de usuario actualizado correctamente" });
        }
    }
}