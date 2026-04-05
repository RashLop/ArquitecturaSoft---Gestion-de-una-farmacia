using System.Collections.Generic;
using System.Text.RegularExpressions;
using ProyectoArqSoft.DTO;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioActualizacionValidacion : IResult<UsuarioActualizacionDto>
    {
        private static readonly HashSet<string> ExtensionesValidas = new HashSet<string>
        {
            "LP", "CB", "SC", "OR", "PT", "TJ", "CH", "BE", "PD"
        };

        public Result Validar(UsuarioActualizacionDto dto)
        {
            if (dto == null)
                return Result.Fail("Datos nulos.");

            return
                ValidarNombres(dto.Nombres) ??
                ValidarPrimerApellido(dto.ApellidoPaterno) ??
                ValidarSegundoApellido(dto.ApellidoMaterno) ??
                ValidarCi(dto.Ci) ??
                ValidarCiExtencion(dto.CiExtencion) ??
                ValidarTelefono(dto.Telefono) ??
                ValidarEmail(dto.Email) ??
                ValidarUserName(dto.UserName) ??
                Result.Ok();
        }

        private Result? ValidarNombres(string nombres)
        {
            if (string.IsNullOrWhiteSpace(nombres))
                return Result.Fail("Los nombres son obligatorios.");

            if (nombres.Trim().Length > 45)
                return Result.Fail("Los nombres no pueden tener más de 45 caracteres.");

            return null;
        }

        private Result? ValidarPrimerApellido(string apellidoPaterno)
        {
            if (string.IsNullOrWhiteSpace(apellidoPaterno))
                return Result.Fail("El primer apellido es obligatorio.");

            if (apellidoPaterno.Trim().Length > 45)
                return Result.Fail("El primer apellido no puede tener más de 45 caracteres.");

            return null;
        }

        private Result? ValidarSegundoApellido(string apellidoMaterno)
        {
            if (string.IsNullOrWhiteSpace(apellidoMaterno))
                return null;

            if (apellidoMaterno.Trim().Length > 45)
                return Result.Fail("El segundo apellido no puede tener más de 45 caracteres.");

            return null;
        }

        private Result? ValidarCi(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci))
                return Result.Fail("El número de carnet es obligatorio.");

            ci = ci.Trim();

            if (ci.Contains(' '))
                return Result.Fail("El número de carnet no debe contener espacios.");

            if (!Regex.IsMatch(ci, @"^\d{5,10}(-\d+[A-Za-z])?$"))
                return Result.Fail("El CI debe tener de 5 a 10 dígitos y formato válido, por ejemplo: 1234567 o 1234567-1A.");

            return null;
        }

        private Result? ValidarCiExtencion(string ciExtencion)
        {
            if (string.IsNullOrWhiteSpace(ciExtencion))
                return Result.Fail("La extensión del CI es obligatoria.");

            ciExtencion = ciExtencion.Trim().ToUpper();

            if (!ExtensionesValidas.Contains(ciExtencion))
                return Result.Fail("La extensión del CI no es válida.");

            return null;
        }

        private Result? ValidarTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return Result.Fail("El teléfono es obligatorio.");

            if (telefono.Trim().Length > 45)
                return Result.Fail("El teléfono no puede tener más de 45 caracteres.");

            if (!Regex.IsMatch(telefono, @"^[0-9+\-\s]+$"))
                return Result.Fail("El teléfono solo puede contener números, espacios, '+' y '-'.");

            return null;
        }

        private Result? ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Fail("El email es obligatorio.");

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result.Fail("Email no válido.");

            return null;
        }

        private Result? ValidarUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return Result.Fail("Username obligatorio.");

            if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9._]+$"))
                return Result.Fail("El username solo permite letras, números, puntos y guion bajo.");

            return null;
        }
    }
}
