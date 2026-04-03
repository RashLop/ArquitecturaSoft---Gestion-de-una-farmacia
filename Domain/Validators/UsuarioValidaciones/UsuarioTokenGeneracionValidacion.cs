using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioTokenGeneracionValidacion : IValidacion<UsuarioTokenGeneracionDto>
    {
        public Validacion Validar(UsuarioTokenGeneracionDto dto)
        {
            if (dto == null)
                return Validacion.Fail("Los datos del token no pueden ser nulos.");

            if (dto.IdUsuario <= 0)
                return Validacion.Fail("El id del usuario debe ser mayor a cero.");

            string tipoToken = dto.TipoToken?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(tipoToken))
                return Validacion.Fail("El tipo de token es obligatorio.");

            if (tipoToken.Length > 50)
                return Validacion.Fail("El tipo de token no puede tener más de 50 caracteres.");

            if (dto.MinutosExpiracion <= 0)
                return Validacion.Fail("Los minutos de expiración deben ser mayores a cero.");

            return Validacion.Ok();
        }
    }
}