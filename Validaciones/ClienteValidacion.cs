using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    public class ClienteValidacion : IValidacion<Cliente>
    {
        public Validacion Validar(Cliente cliente)
        {
            if (EsConsumidorFinal(cliente))
                return new Validacion(true);

            if (string.IsNullOrWhiteSpace(cliente.Nit))
                return new Validacion(false, "El NIT es obligatorio.");

            if (cliente.Nit.Any(char.IsWhiteSpace))
                return new Validacion(false, "El NIT no debe contener espacios.");

            if (!Regex.IsMatch(cliente.Nit, @"^\d+$"))
                return new Validacion(false, "El NIT solo debe contener numeros. No se permiten letras ni caracteres especiales.");

            if (cliente.Nit.Length < 5 || cliente.Nit.Length > 20)
                return new Validacion(false, "El NIT debe tener entre 5 y 20 digitos.");

            if (cliente.Nit.All(c => c == '0'))
                return new Validacion(false, "El NIT no puede estar compuesto solo por ceros.");

            if (string.IsNullOrWhiteSpace(cliente.RazonSocial))
                return new Validacion(false, "La razon social es obligatoria.");

            if (cliente.RazonSocial.Length < 3 || cliente.RazonSocial.Length > 45)
                return new Validacion(false, "La razon social debe tener entre 3 y 45 caracteres.");

            if (!string.IsNullOrWhiteSpace(cliente.CorreoElectronico))
            {
                if (cliente.CorreoElectronico.Length > 45)
                    return new Validacion(false, "El correo electronico no puede superar los 45 caracteres.");

                if (!Regex.IsMatch(cliente.CorreoElectronico, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return new Validacion(false, "El correo electronico no tiene un formato valido.");
            }

            return new Validacion(true);
        }

        private static bool EsConsumidorFinal(Cliente cliente)
        {
            return cliente.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase) &&
                   cliente.RazonSocial.Equals("Consumidor Final", StringComparison.OrdinalIgnoreCase);
        }
    }
}
