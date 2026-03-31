using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    public class ClienteValidacion : IValidacion<Cliente>
    {
        public Validacion Validar(Cliente cliente)
        {
            if (EsConsumidorFinal(cliente))
                return Validacion.Ok();

            return ValidarNit(cliente.Nit)
                ?? ValidarRazonSocial(cliente.RazonSocial)
                ?? ValidarCorreoElectronico(cliente.CorreoElectronico)
                ?? Validacion.Ok();
        }

        private Validacion? ValidarNit(string nit)
        {
            if (string.IsNullOrWhiteSpace(nit))
                return Validacion.Fail("El NIT es obligatorio.");

            if (nit.Any(char.IsWhiteSpace))
                return Validacion.Fail("El NIT no debe contener espacios.");

            if (!Regex.IsMatch(nit, @"^\d+$"))
                return Validacion.Fail("El NIT solo debe contener numeros y el formato correcto (ej: 123456789).");

            if (nit.Length < 5 || nit.Length > 20)
                return Validacion.Fail("El NIT debe tener de 5 a 20 digitos y el formato correcto (ej: 123456789).");

            if (nit.All(c => c == '0'))
                return Validacion.Fail("El NIT no puede estar compuesto solo por ceros.");

            return null;
        }

        private Validacion? ValidarRazonSocial(string razonSocial)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                return Validacion.Fail("La razon social es obligatoria.");

            if (razonSocial.Length < 3 || razonSocial.Length > 45)
                return Validacion.Fail("La razon social debe tener entre 3 y 45 caracteres.");

            return null;
        }

        private Validacion? ValidarCorreoElectronico(string? correoElectronico)
        {
            if (string.IsNullOrWhiteSpace(correoElectronico))
                return null;

            if (correoElectronico.Length > 45)
                return Validacion.Fail("El correo electronico no puede superar los 45 caracteres.");

            if (!Regex.IsMatch(correoElectronico, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Validacion.Fail("El correo electronico no tiene un formato valido.");

            return null;
        }

        private static bool EsConsumidorFinal(Cliente cliente)
        {
            return cliente.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase) &&
                   cliente.RazonSocial.Equals("Consumidor Final", StringComparison.OrdinalIgnoreCase);
        }
    }
}