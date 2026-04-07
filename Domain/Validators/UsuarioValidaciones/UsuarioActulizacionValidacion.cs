using ProyectoArqSoft.Domain.DTOs;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioActualizacionValidacion : UsuarioValidacionBase, IResult<UsuarioActualizacionDto>
    {
        public Result Validar(UsuarioActualizacionDto dto)
        {
            if (dto == null)
                return Result.Fail("Datos nulos.");

            if (dto.IdUsuario <= 0)
                return Result.Fail("El identificador del usuario no es válido.");

            return EmailValido(dto.Email) ?? Result.Ok();
        }
    }
}