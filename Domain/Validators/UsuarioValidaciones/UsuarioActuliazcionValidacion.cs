using System.Text.RegularExpressions;
using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioActualizacionValidacion : IValidacion<UsuarioActualizacionDto>
    {
        public Validacion Validar(UsuarioActualizacionDto dto)
        {
            if (dto == null) return Validacion.Fail("Datos nulos.");

            return
                ValidarEmail(dto.Email) ??
                ValidarUserName(dto.UserName) ??
                Validacion.Ok();
        }

        private Validacion? ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return Validacion.Fail("El email es obligatorio.");
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return Validacion.Fail("Email no válido.");
            return null;
        }

        private Validacion? ValidarUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return Validacion.Fail("Username obligatorio.");
            // CAMBIO: Se agregó el punto "." a la expresión regular
            if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9._]+$"))
                return Validacion.Fail("El username solo permite letras, números, puntos y guion bajo.");
            return null;
        }
    }
}