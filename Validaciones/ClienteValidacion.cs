using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    public class ClienteValidacion : IValidacion<Cliente>
    {
        private const int MAX_RAZON_SOCIAL = 36;
        private const int MIN_RAZON_SOCIAL = 10;

        public Validacion Validar(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nit))
                return new Validacion(false, "El NIT es obligatorio");

            if (!Regex.IsMatch(cliente.Nit, @"^\d+$"))
                return new Validacion(false, "El NIT debe contener solo números");

            if (cliente.Nit.Length < 5 || cliente.Nit.Length > 15)
                return new Validacion(false, "El NIT debe tener entre 5 y 15 dígitos");

            if (string.IsNullOrWhiteSpace(cliente.RazonSocial))
                return new Validacion(false, "La razón social es obligatoria");

            string razonSocialNormalizada = NormalizarEspacios(cliente.RazonSocial);
            cliente.RazonSocial = razonSocialNormalizada;

            if (cliente.RazonSocial.Length < MIN_RAZON_SOCIAL)
                return new Validacion(false, $"La razón social debe tener al menos {MIN_RAZON_SOCIAL} caracteres (sin contar espacios extras)");

            if (cliente.RazonSocial.Length > MAX_RAZON_SOCIAL)
                return new Validacion(false, $"La razón social no puede tener más de {MAX_RAZON_SOCIAL} caracteres");

            if (!Regex.IsMatch(cliente.RazonSocial, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\.\-]+$"))
                return new Validacion(false, "La razón social solo puede contener letras, espacios, puntos y guiones");

            if (!string.IsNullOrWhiteSpace(cliente.CorreoElectronico))
            {
                if (!Regex.IsMatch(cliente.CorreoElectronico, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return new Validacion(false, "El formato del correo electrónico no es válido");
            }

            if (cliente.FechaRegistro == DateTime.MinValue)
                return new Validacion(false, "La fecha de registro es obligatoria");

            if (cliente.FechaRegistro > DateTime.Today)
                return new Validacion(false, "La fecha de registro no puede ser futura");

            return new Validacion(true);
        }
        private string NormalizarEspacios(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return Regex.Replace(input.Trim(), @"\s+", " ");
        }
    }
}