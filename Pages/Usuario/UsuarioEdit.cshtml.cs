using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Domain.DTOs;

namespace ProyectoArqSoft.Pages.Usuario
{
    public class UsuarioEditModel : BasePageModel
    {
        private readonly IUsuarioService _service;

        [BindProperty]
        public UsuarioEdicionDto Input { get; set; } = new();

        public UsuarioEditModel(IUsuarioService service) => _service = service;

        public IActionResult OnPostCargarUsuarioParaEdicion(int id)
        {
            var user = _service.ObtenerUsuarioPorId(id);
            if (user == null)
                return RedirectToPage("Usuario");

            Input = new UsuarioEdicionDto
            {
                IdUsuario = user.IdUsuario,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role,
                Activo = user.Activo
            };

            return Page();
        }

        public IActionResult OnPostActualizarUsuario()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = _service.ActualizarUsuarioEdicion(Input);

            if (result.IsSuccess)
                return RedirectToPage("Usuario", new { mensaje = "Perfil actualizado correctamente" });

            Estado.MensajeError = result.Error;
            return Page();
        }
    }
}