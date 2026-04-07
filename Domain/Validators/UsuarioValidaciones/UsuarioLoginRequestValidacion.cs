using ProyectoArqSoft.Domain.DTOs;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioLoginRequestValidacion : UsuarioValidacionBase, IResult<UsuarioLoginRequestDto>
    {
        public Result Validar(UsuarioLoginRequestDto dto)
        {
            if (dto == null)
                return Result.Fail("Los datos de login no pueden ser nulos.");

            string emailOUser = dto.EmailOUserName?.Trim() ?? string.Empty;
            string password = dto.Password ?? string.Empty;

            return Requerido(emailOUser, "Debe ingresar su email o nombre de usuario.")
                ?? Requerido(password, "Debe ingresar la contraseña.")
                ?? Result.Ok();
        }
    }
}