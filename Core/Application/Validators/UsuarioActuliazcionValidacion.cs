using System.Text.RegularExpressions;
using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioActualizacionValidacion : IValidacion<UsuarioActualizacionDto>
    {
        public Validacion Validar(UsuarioActualizacionDto dto)
        {
            if (dto == null)
                return Validacion.Fail("Los datos del usuario no pueden ser nulos.");

            string email = dto.Email?.Trim() ?? string.Empty;
            string userName = dto.UserName?.Trim() ?? string.Empty;
            string role = dto.Role?.Trim() ?? string.Empty;

            return
                ValidarId(dto.IdUsuario) ??
                ValidarEmail(email) ??
                ValidarUserName(userName) ??
                ValidarRole(role) ??
                Validacion.Ok();
        }

        private Validacion? ValidarId(int id)
        {
            if (id <= 0)
                return Validacion.Fail("El identificador del usuario no es válido.");

            return null;
        }

        private Validacion? ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Validacion.Fail("El correo electrónico es obligatorio.");

            if (email.Length > 100)
                return Validacion.Fail("El correo electrónico no puede tener más de 100 caracteres.");

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Validacion.Fail("El formato del correo electrónico no es válido.");

            return null;
        }

        private Validacion? ValidarUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return Validacion.Fail("El nombre de usuario es obligatorio.");

            if (userName.Length < 3 || userName.Length > 30)
                return Validacion.Fail("El nombre de usuario debe tener entre 3 y 30 caracteres.");

            if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9_]+$"))
                return Validacion.Fail("El nombre de usuario solo puede contener letras, números y guion bajo.");

            return null;
        }

        private Validacion? ValidarRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return Validacion.Fail("El rol es obligatorio.");

            if (role.Length > 20)
                return Validacion.Fail("El rol no puede tener más de 20 caracteres.");

            return null;
        }
    }
}