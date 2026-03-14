using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    public class ClienteValidacion : IValidacion<Cliente>
    {
        private static readonly string[] ExtensionesValidas = ["LP", "CB", "SC", "OR", "PT", "CH", "TJ", "BE", "PD"];

        public Validacion Validar(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.TipoCliente))
                return new Validacion(false, "El tipo de cliente es obligatorio");

            if (!EsTextoValido(cliente.Nombre, 3, 20))
                return new Validacion(false, "El nombre debe tener entre 3 y 45 caracteres y solo letras");

            if (!EsTextoValido(cliente.ApellidoMaterno, 3, 20))
                return new Validacion(false, "El apellido materno debe tener entre 3 y 45 caracteres y solo letras");

            if (!EsTextoValido(cliente.ApellidoPaterno, 3, 20))
                return new Validacion(false, "El apellido paterno debe tener entre 3 y 45 caracteres y solo letras");

            if (string.IsNullOrWhiteSpace(cliente.Ci))
                return new Validacion(false, "El numero de carnet es obligatorio");

            if (cliente.Ci.Contains(' '))
                return new Validacion(false, "El numero de carnet no debe contener espacios");

            if (!Regex.IsMatch(cliente.Ci, @"^\d{5,10}(-\d+[A-Za-z])?$"))
                return new Validacion(false, "El CI debe tener de 5 a 10 digitos y el complemento, si existe, debe seguir el formato 1234567-1A");

            if (string.IsNullOrWhiteSpace(cliente.CiExtencion))
                return new Validacion(false, "La extension del CI es obligatoria");

            if (!ExtensionesValidas.Contains(cliente.CiExtencion))
                return new Validacion(false, "La extension del CI no es valida");

            if (cliente.FechaDeNacimiento == default)
                return new Validacion(false, "La fecha de nacimiento es obligatoria");

            int edad = CalcularEdad(cliente.FechaDeNacimiento);
            if (edad < 18 || edad > 90)
                return new Validacion(false, "La fecha de nacimiento debe corresponder a una edad entre 18 y 90 anos");

            if (!string.IsNullOrWhiteSpace(cliente.Telefono))
            {
                if (cliente.Telefono.Length > 8)
                    return new Validacion(false, "El telefono no puede superar 45 caracteres");

                if (!Regex.IsMatch(cliente.Telefono, @"^[\d\s\+\-\(\)]+$"))
                    return new Validacion(false, "El telefono contiene caracteres invalidos");
            }

            return new Validacion(true);
        }

        private static bool EsTextoValido(string valor, int minimo, int maximo)
        {
            return !string.IsNullOrWhiteSpace(valor)
                && valor.Length >= minimo
                && valor.Length <= maximo
                && Regex.IsMatch(valor, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");
        }

        private static int CalcularEdad(DateTime fechaNacimiento)
        {
            DateTime hoy = DateTime.Today;
            int edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento.Date > hoy.AddYears(-edad))
                edad--;

            return edad;
        }
    }
}
