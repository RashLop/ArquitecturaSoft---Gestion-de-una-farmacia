using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages.Usuario
{
    public class UsuarioEditModel : BasePageModel
    {
        private readonly IUsuarioService _usuarioService;

        [BindProperty]
        public UsuarioActualizacionDto Input { get; set; } = new();

        public UsuarioEditModel(IUsuarioService usuarioService) => _usuarioService = usuarioService;

        public IActionResult OnPostCargarUsuarioParaEdicion(int id)
        {
            var user = _usuarioService.ObtenerUsuarioPorId(id);
            if (user == null) return RedirectToPage("Usuario", new { error = "Usuario no encontrado" });

            // Cargamos todos los datos para mantener la integridad
            Input.IdUsuario = user.IdUsuario;
            Input.UserName = user.UserName;
            Input.Email = user.Email;
            Input.Role = user.Role;

            // Estos campos son obligatorios para el DTO, los recuperamos de la base de datos
            // Nota: Asegúrate de que tu servicio 'ObtenerUsuarioPorId' devuelva estos campos reales
            Input.Nombres = user.UserName; // Placeholder si no tienes el campo en el DTO de salida
            Input.ApellidoPaterno = "Actualización";

            return Page();
        }

        public IActionResult OnPostActualizarUsuario()
        {
            // Ejecutamos la actualización usando el objeto Result
            Result resultado = _usuarioService.ActualizarUsuario(Input);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Usuario", new { mensaje = "Perfil de usuario actualizado correctamente" });
        }
    }
}