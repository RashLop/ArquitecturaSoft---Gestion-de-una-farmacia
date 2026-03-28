using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    public class BioquimicoFormularioValidacion : IValidacion<Bioquimico>
    {
        private static readonly string[] ExtensionesValidas =
        [
            "LP", "CB", "SC", "OR", "PT", "CH", "TJ", "BE", "PD"
        ];

        public Validacion Validar(Bioquimico bioquimico)
        {
            return ValidarNombres(bioquimico.Nombres)
                ?? ValidarApellidoMaterno(bioquimico.ApellidoMaterno)
                ?? ValidarApellidoPaterno(bioquimico.ApellidoPaterno)
                ?? ValidarCi(bioquimico.Ci)
                ?? ValidarCiExtension(bioquimico.CiExtencion)
                ?? ValidarTelefono(bioquimico.Telefono)
                ?? Validacion.Ok();
        }

        private Validacion? ValidarNombres(string nombres)
        {
            return EsTextoValido(nombres, 3, 45)
                ? null
                : Validacion.Fail("El nombre debe tener entre 3 y 45 caracteres y solo letras");
        }

        private Validacion? ValidarApellidoMaterno(string apellidoMaterno)
        {
            return EsTextoValido(apellidoMaterno, 3, 45)
                ? null
                : Validacion.Fail("El apellido materno debe tener entre 3 y 45 caracteres y solo letras");
        }

        private Validacion? ValidarApellidoPaterno(string apellidoPaterno)
        {
            return EsTextoValido(apellidoPaterno, 3, 45)
                ? null
                : Validacion.Fail("El apellido paterno debe tener entre 3 y 45 caracteres y solo letras");
        }

        private Validacion? ValidarCi(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci))
                return Validacion.Fail("El número de carnet es obligatorio");

            if (ci.Contains(' '))
                return Validacion.Fail("El número de carnet no debe contener espacios");

            if (!Regex.IsMatch(ci, @"^\d{5,10}(-\d+[A-Za-z])?$"))
                return Validacion.Fail("El CI debe tener de 5 a 10 dígitos y el formato correcto (ej: 1234567 o 1234567-1A)");

            return null;
        }

        private Validacion? ValidarCiExtension(string ciExtencion)
        {
            if (string.IsNullOrWhiteSpace(ciExtencion))
                return Validacion.Fail("La extensión del CI es obligatoria");

            if (!ExtensionesValidas.Contains(ciExtencion))
                return Validacion.Fail("La extensión del CI no es válida");

            return null;
        }

        private Validacion? ValidarTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return Validacion.Fail("El teléfono es obligatorio");

            if (!Regex.IsMatch(telefono, @"^\d{8}$"))
                return Validacion.Fail("El teléfono debe tener exactamente 8 dígitos");

            return null;
        }

        private static bool EsTextoValido(string valor, int minimo, int maximo)
        {
            return !string.IsNullOrWhiteSpace(valor)
                && valor.Length >= minimo
                && valor.Length <= maximo
                && Regex.IsMatch(valor, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");
        }
    }

    public class BioquimicoBusquedasValidacion : IValidacion<string>
    {
        public Validacion Validar(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Validacion.Ok();

            filtro = filtro.Trim();

            return ValidarLongitud(filtro)
                ?? ValidarFormatoBusqueda(filtro)
                ?? Validacion.Ok();
        }

        private Validacion? ValidarLongitud(string filtro)
        {
            return filtro.Length > 20
                ? Validacion.Fail("El criterio es demasiado largo")
                : null;
        }

        private Validacion? ValidarFormatoBusqueda(string filtro)
        {
            string patronValido = @"^([a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+|\d+|\d+-\d[A-Z])$";

            return Regex.IsMatch(filtro, patronValido)
                ? null
                : Validacion.Fail("Criterio de búsqueda inválido");
        }
    }
}