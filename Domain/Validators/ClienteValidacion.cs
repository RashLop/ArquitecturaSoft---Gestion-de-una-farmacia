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
            if (string.IsNullOrWhiteSpace(nit))
                return Result.Fail("El NIT es obligatorio.");

            if (nit.Any(char.IsWhiteSpace))
                return Result.Fail("El NIT no debe contener espacios.");

            if (!Regex.IsMatch(nit, @"^\d+$"))
                return Result.Fail("El NIT solo debe contener numeros y el formato correcto (ej: 123456789).");

            if (nit.Length < 5 || nit.Length > 20)
                return Result.Fail("El NIT debe tener de 5 a 20 digitos y el formato correcto (ej: 123456789).");

            if (nit.All(c => c == '0'))
                return Result.Fail("El NIT no puede estar compuesto solo por ceros.");

            return null;
        }

        private Result? ValidarRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return Result.Fail("La razon social es obligatoria.");

            if (razonSocial.Length < 3 || razonSocial.Length > 45)
                return Result.Fail("La razon social debe tener entre 3 y 45 caracteres.");

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
            return cliente.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase) &&
                   cliente.RazonSocial.Equals("Consumidor Final", StringComparison.OrdinalIgnoreCase);
        }
    }
}