using System.Text.RegularExpressions;
using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Application.Interfaces;

namespace ProyectoArqSoft.Domain.Validators
{
    public class ClienteValidacion : IResult<Cliente>
    {
        public Result Validar(Cliente cliente)
        {
            if (EsConsumidorFinal(cliente))
                return Result.Ok();

            return ValidarNit(cliente.Nit)
                ?? ValidarRazonSocial(cliente.RazonSocial)
                ?? ValidarCorreoElectronico(cliente.CorreoElectronico)
                ?? Result.Ok();
        }

        private Result? ValidarNit(string nit)
        {
            return ValidarNitObligatorio(nit)
                ?? ValidarNitFormatoBasico(nit)
                ?? ValidarNitContenido(nit)
                ?? ValidarNitLongitud(nit)
                ?? ValidarNitPatron(nit)
                ?? ValidarNitNoSeaSoloCeros(nit);
        }

        private Result? ValidarRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return Result.Fail("La razon social es obligatoria.");

            if (!razonSocial.Equals(razonSocial.Trim(), StringComparison.Ordinal))
                return Result.Fail("La razon social no debe contener espacios al inicio o al final.");

            if (razonSocial.Any(c => c == '\t' || c == '\n' || c == '\r'))
                return Result.Fail("La razon social no debe contener tabs ni saltos de linea.");

            if (razonSocial.Length < 3 || razonSocial.Length > 45)
                return Result.Fail("La razon social debe tener entre 3 y 45 caracteres.");

            if (!Regex.IsMatch(razonSocial, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\.\-&]+$"))
                return Result.Fail("La razon social contiene caracteres no permitidos.");

            if (!razonSocial.Any(char.IsLetterOrDigit))
                return Result.Fail("La razon social debe contener al menos letras o numeros.");

            return null;
        }

        private Result? ValidarCorreoElectronico(string? correoElectronico)
        {
            if (string.IsNullOrWhiteSpace(correoElectronico))
                return null;

            if (correoElectronico.Length > 45)
                return Result.Fail("El correo electronico no puede superar los 45 caracteres.");

            if (!Regex.IsMatch(correoElectronico, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result.Fail("El correo electronico no tiene un formato valido.");

            return null;
        }

        private static bool EsConsumidorFinal(Cliente cliente)
        {
            string nit = cliente.Nit ?? string.Empty;
            string razonSocial = cliente.RazonSocial ?? string.Empty;

            return nit.Equals("CF", StringComparison.OrdinalIgnoreCase) &&
                   razonSocial.Equals("Consumidor Final", StringComparison.OrdinalIgnoreCase);
        }

        private static Result? ValidarNitObligatorio(string nit)
        {
            if (nit == null || nit.Length == 0)
                return Result.Fail("El NIT es obligatorio.");

            if (string.IsNullOrWhiteSpace(nit))
                return Result.Fail("El NIT no debe contener solo espacios.");

            return null;
        }

        private static Result? ValidarNitFormatoBasico(string nit)
        {
            if (nit.Any(c => c == '\t' || c == '\n' || c == '\r'))
                return Result.Fail("El NIT no debe contener tabs ni saltos de linea.");

            if (!nit.Equals(nit.Trim(), StringComparison.Ordinal))
                return Result.Fail("El NIT no debe contener espacios al inicio o al final.");

            if (nit.Contains(' '))
                return Result.Fail("El NIT no debe contener espacios internos.");

            if (nit.StartsWith("+") || nit.StartsWith("-"))
                return Result.Fail("El NIT no debe contener signos positivos ni negativos.");

            if (Regex.IsMatch(nit, @"^\d+\.\d+$"))
                return Result.Fail("El NIT no debe contener numeros decimales.");

            return null;
        }

        private static Result? ValidarNitContenido(string nit)
        {
            bool contieneLetras = nit.Any(char.IsLetter);
            bool contieneEspacios = nit.Any(char.IsWhiteSpace);
            bool contieneSimbolos = nit.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c));

            if (contieneLetras && contieneSimbolos)
                return Result.Fail("El NIT no debe mezclar letras con simbolos o caracteres especiales.");

            if (contieneLetras && contieneEspacios)
                return Result.Fail("El NIT no debe contener texto ni espacios.");

            if (contieneLetras)
                return Result.Fail("El NIT solo debe contener numeros, no letras.");

            if (contieneSimbolos)
                return Result.Fail("El NIT no debe contener simbolos ni caracteres especiales.");

            return null;
        }

        private static Result? ValidarNitLongitud(string nit)
        {
            if (nit.Length < 5)
                return Result.Fail("El NIT debe contener entre 5 y 12 digitos; faltan digitos.");

            if (nit.Length > 12)
                return Result.Fail("El NIT debe contener entre 5 y 12 digitos; sobran digitos.");

            return null;
        }

        private static Result? ValidarNitPatron(string nit)
        {
            if (!Regex.IsMatch(nit, @"^\d{5,12}$"))
                return Result.Fail("El NIT debe contener entre 5 y 12 digitos numericos.");

            return null;
        }

        private static Result? ValidarNitNoSeaSoloCeros(string nit)
        {
            if (nit.All(c => c == '0'))
                return Result.Fail("El NIT no puede estar compuesto solo por ceros.");

            return null;
        }
    }
}

