using System.Text.RegularExpressions;
using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioRegistroValidacion : IValidacion<UsuarioRegistroDto>
    {
        public Validacion Validar(UsuarioRegistroDto dto)
        {
            if (dto == null)
                return Validacion.Fail("Los datos del usuario no pueden ser nulos.");

            string email = dto.Email?.Trim() ?? string.Empty;
            string userName = dto.UserName?.Trim() ?? string.Empty;
            string password = dto.Password ?? string.Empty;
            string confirmarPassword = dto.ConfirmarPassword ?? string.Empty;

            return
                ValidarEmail(email) ??
                ValidarUserName(userName) ??
                ValidarPassword(password) ??
                ValidarConfirmacionPassword(password, confirmarPassword) ??
                Validacion.Ok();
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

        private Validacion? ValidarPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Validacion.Fail("La contraseña es obligatoria.");

            if (password.Length < 8)
                return Validacion.Fail("La contraseña debe tener al menos 8 caracteres.");

            if (password.Length > 50)
                return Validacion.Fail("La contraseña no puede tener más de 50 caracteres.");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return Validacion.Fail("La contraseña debe contener al menos una letra mayúscula.");

            if (!Regex.IsMatch(password, @"[a-z]"))
                return Validacion.Fail("La contraseña debe contener al menos una letra minúscula.");

            if (!Regex.IsMatch(password, @"\d"))
                return Validacion.Fail("La contraseña debe contener al menos un número.");

            if (!Regex.IsMatch(password, @"[\W_]"))
                return Validacion.Fail("La contraseña debe contener al menos un carácter especial.");

            return null;
        }

        private Validacion? ValidarConfirmacionPassword(string password, string confirmarPassword)
        {
            if (string.IsNullOrWhiteSpace(confirmarPassword))
                return Validacion.Fail("Debe confirmar la contraseña.");

            if (password != confirmarPassword)
                return Validacion.Fail("La contraseña y su confirmación no coinciden.");

            return null;
        }
    }
}