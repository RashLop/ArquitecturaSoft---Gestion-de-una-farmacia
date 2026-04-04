using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioTokenGeneracionValidacion : IResult<UsuarioTokenGeneracionDto>
    {
        public Result Validar(UsuarioTokenGeneracionDto dto)
        {
            if (dto == null)
                return Result.Fail("Los datos del token no pueden ser nulos.");

            if (dto.IdUsuario <= 0)
                return Result.Fail("El id del usuario debe ser mayor a cero.");

            string tipoToken = dto.TipoToken?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(tipoToken))
                return Result.Fail("El tipo de token es obligatorio.");

            if (tipoToken.Length > 50)
                return Result.Fail("El tipo de token no puede tener más de 50 caracteres.");

            if (dto.MinutosExpiracion <= 0)
                return Result.Fail("Los minutos de expiración deben ser mayores a cero.");

            return Result.Ok();
        }
    }
}