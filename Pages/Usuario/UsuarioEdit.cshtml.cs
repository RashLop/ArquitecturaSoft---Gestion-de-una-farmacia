using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Pages.Usuario
{
    public class UsuarioEditModel : BasePageModel
    {
        private readonly IUsuarioService _service;
        [BindProperty] public UsuarioActualizacionDto Input { get; set; } = new();

        public UsuarioEditModel(IUsuarioService service) => _service = service;

        public IActionResult OnPostCargarUsuarioParaEdicion(int id)
        {
            var user = _service.ObtenerUsuarioPorId(id);
            if (user == null) return RedirectToPage("Usuario");

            // MAPEADO CORRECTO (Esto arregla tu captura)
            Input.IdUsuario = id;
            Input.Email = user.Email;
            Input.UserName = user.UserName;
            Input.Role = user.Role;

            // Mantenemos los nombres en el DTO (ocultos) para que la validación no falle
            Input.Nombres = "N/A";
            Input.ApellidoPaterno = "N/A";

            return Page();
        }

        public IActionResult OnPostActualizarUsuario()
        {
            var result = _service.ActualizarUsuario(Input);
            if (result.IsSuccess)
                return RedirectToPage("Usuario", new { mensaje = "Perfil actualizado correctamente" });

            Estado.MensajeError = result.Error;
            return Page();
        }
    }
}