using System.Text.RegularExpressions;
using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioActualizacionValidacion : IResult<UsuarioActualizacionDto>
    {
              public Result Validar(UsuarioActualizacionDto dto)
        {
            if (dto == null) return Result.Fail("Datos nulos.");

            return
                ValidarEmail(dto.Email) ??
                ValidarUserName(dto.UserName) ??
                Result.Ok();
        }

        private Result? ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return Result.Fail("El email es obligatorio.");
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return Result.Fail("Email no válido.");
            return null;
        }

        private Result? ValidarUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return Result.Fail("Username obligatorio.");
            // CAMBIO: Se agregó el punto "." a la expresión regular
            if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9._]+$"))
                return Result.Fail("El username solo permite letras, números, puntos y guion bajo.");
            return null;
        }
    }
}