using System.Text.RegularExpressions;
using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioActualizacionValidacion : IResult<UsuarioActualizacionDto>
    {
        public Result Validar(UsuarioActualizacionDto dto)
        {
            if (dto == null)
                return Result.Fail("Los datos del usuario no pueden ser nulos.");

            string email = dto.Email?.Trim() ?? string.Empty;
            string userName = dto.UserName?.Trim() ?? string.Empty;
            string role = dto.Role?.Trim() ?? string.Empty;

            return
                ValidarId(dto.IdUsuario) ??
                ValidarEmail(email) ??
                ValidarUserName(userName) ??
                ValidarRole(role) ??
                Result.Ok();
        }

        private Result? ValidarId(int id)
        {
            if (id <= 0)
                return Result.Fail("El identificador del usuario no es válido.");

            return null;
        }

        private Result? ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Fail("El correo electrónico es obligatorio.");

            if (email.Length > 100)
                return Result.Fail("El correo electrónico no puede tener más de 100 caracteres.");

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result.Fail("El formato del correo electrónico no es válido.");

            return null;
        }

        private Result? ValidarUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return Result.Fail("El nombre de usuario es obligatorio.");

            if (userName.Length < 3 || userName.Length > 30)
                return Result.Fail("El nombre de usuario debe tener entre 3 y 30 caracteres.");

            if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9_]+$"))
                return Result.Fail("El nombre de usuario solo puede contener letras, números y guion bajo.");

            return null;
        }

        private Result? ValidarRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return Result.Fail("El rol es obligatorio.");

            if (role.Length > 20)
                return Result.Fail("El rol no puede tener más de 20 caracteres.");

            return null;
        }
    }
}