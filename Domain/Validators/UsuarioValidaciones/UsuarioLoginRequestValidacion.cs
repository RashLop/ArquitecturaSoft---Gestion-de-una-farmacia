using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioLoginRequestValidacion : IResult<UsuarioLoginRequestDto>
    {
        public Result Validar(UsuarioLoginRequestDto dto)
        {
            if (dto == null)
                return Result.Fail("Los datos de login no pueden ser nulos.");

            string emailOUser = dto.EmailOUserName?.Trim() ?? string.Empty;
            string password = dto.Password ?? string.Empty;

            if (string.IsNullOrWhiteSpace(emailOUser))
                return Result.Fail("Debe ingresar su email o nombre de usuario.");

            if (emailOUser.Length < 3)
                return Result.Fail("El email o nombre de usuario es demasiado corto.");

            if (string.IsNullOrWhiteSpace(password))
                return Result.Fail("Debe ingresar la contraseña.");

            if (password.Length < 6)
                return Result.Fail("La contraseña no es válida.");

            return Result.Ok();
        }
    }
}