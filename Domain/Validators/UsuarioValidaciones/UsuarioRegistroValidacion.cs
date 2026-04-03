using System.Text.RegularExpressions;
using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioRegistroValidacion : IValidacion<UsuarioRegistroDto>
    {
        private static readonly HashSet<string> ExtensionesValidas = new HashSet<string>
        {
            "LP", "CB", "SC", "OR", "PT", "TJ", "CH", "BE", "PD"
        };

        public Validacion Validar(UsuarioRegistroDto dto)
        {
            if (dto == null)
                return Validacion.Fail("Los datos del usuario no pueden ser nulos.");

            string nombres = dto.Nombres?.Trim() ?? string.Empty;
            string apellidoPaterno = dto.ApellidoPaterno?.Trim() ?? string.Empty;
            string apellidoMaterno = dto.ApellidoMaterno?.Trim() ?? string.Empty;
            string ci = dto.Ci?.Trim() ?? string.Empty;
            string ciExtencion = dto.CiExtencion?.Trim() ?? string.Empty;
            string telefono = dto.Telefono?.Trim() ?? string.Empty;
            string email = dto.Email?.Trim() ?? string.Empty;

            return
                ValidarNombres(nombres) ??
                ValidarApellidoPaterno(apellidoPaterno) ??
                ValidarApellidoMaterno(apellidoMaterno) ??
                ValidarCi(ci) ??
                ValidarCiExtencion(ciExtencion) ??
                ValidarTelefono(telefono) ??
                ValidarEmail(email) ??
                Validacion.Ok();
        }

        private Validacion? ValidarNombres(string nombres)
        {
            if (string.IsNullOrWhiteSpace(nombres))
                return Validacion.Fail("Los nombres son obligatorios.");

            if (nombres.Length > 45)
                return Validacion.Fail("Los nombres no pueden tener más de 45 caracteres.");

            return null;
        }

        private Validacion? ValidarApellidoPaterno(string apellidoPaterno)
        {
            if (string.IsNullOrWhiteSpace(apellidoPaterno))
                return Validacion.Fail("El apellido paterno es obligatorio.");

            if (apellidoPaterno.Length > 45)
                return Validacion.Fail("El apellido paterno no puede tener más de 45 caracteres.");

            return null;
        }

        private Validacion? ValidarApellidoMaterno(string apellidoMaterno)
        {
            if (string.IsNullOrWhiteSpace(apellidoMaterno))
                return Validacion.Fail("El apellido materno es obligatorio.");

            if (apellidoMaterno.Length > 45)
                return Validacion.Fail("El apellido materno no puede tener más de 45 caracteres.");

            return null;
        }

        private Validacion? ValidarCi(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci))
                return Validacion.Fail("El número de carnet es obligatorio.");

            ci = ci.Trim();

            if (ci.Contains(' '))
                return Validacion.Fail("El número de carnet no debe contener espacios.");

            if (!Regex.IsMatch(ci, @"^\d{5,10}(-\d+[A-Za-z])?$"))
                return Validacion.Fail("El CI debe tener de 5 a 10 dígitos y formato válido, por ejemplo: 1234567 o 1234567-1A.");

            return null;
        }

        private Validacion? ValidarCiExtencion(string ciExtencion)
        {
            if (string.IsNullOrWhiteSpace(ciExtencion))
                return Validacion.Fail("La extensión del CI es obligatoria.");

            ciExtencion = ciExtencion.Trim().ToUpper();

            if (!ExtensionesValidas.Contains(ciExtencion))
                return Validacion.Fail("La extensión del CI no es válida.");

            return null;
        }

        private Validacion? ValidarTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return Validacion.Fail("El teléfono es obligatorio.");

            if (telefono.Length > 45)
                return Validacion.Fail("El teléfono no puede tener más de 45 caracteres.");

            if (!Regex.IsMatch(telefono, @"^[0-9+\-\s]+$"))
                return Validacion.Fail("El teléfono solo puede contener números, espacios, '+' y '-'.");

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
    }
}