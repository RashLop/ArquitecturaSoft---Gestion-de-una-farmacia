using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioLoginRequestValidacion : IValidacion<UsuarioLoginRequestDto>
    {
        public Validacion Validar(UsuarioLoginRequestDto dto)
        {
            if (dto == null)
                return Validacion.Fail("Los datos de login no pueden ser nulos.");

            string emailOUser = dto.EmailOUserName?.Trim() ?? string.Empty;
            string password = dto.Password ?? string.Empty;

            if (string.IsNullOrWhiteSpace(emailOUser))
                return Validacion.Fail("Debe ingresar su email o nombre de usuario.");

            if (emailOUser.Length < 3)
                return Validacion.Fail("El email o nombre de usuario es demasiado corto.");

            if (string.IsNullOrWhiteSpace(password))
                return Validacion.Fail("Debe ingresar la contraseña.");

            if (password.Length < 6)
                return Validacion.Fail("La contraseña no es válida.");

            return Validacion.Ok();
        }
    }
}