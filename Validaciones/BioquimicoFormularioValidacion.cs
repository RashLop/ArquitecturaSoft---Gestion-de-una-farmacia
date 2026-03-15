using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    public class BioquimicoFormularioValidacion : IValidacion<Bioquimico>
    {
        private static readonly string[] ExtensionesValidas = ["LP", "CB", "SC", "OR", "PT", "CH", "TJ", "BE", "PD"];

        public Validacion Validar(Bioquimico bioquimico)
        {
            if (!EsTextoValido(bioquimico.Nombres, 3, 45))
                return new Validacion(false, "El nombre debe tener entre 3 y 45 caracteres y solo letras");

            if (!EsTextoValido(bioquimico.ApellidoMaterno, 3, 45))
                return new Validacion(false, "El apellido materno debe tener entre 3 y 45 caracteres y solo letras");

            if (!EsTextoValido(bioquimico.ApellidoPaterno, 3, 45))
                return new Validacion(false, "El apellido paterno debe tener entre 3 y 45 caracteres y solo letras");

            if (string.IsNullOrWhiteSpace(bioquimico.Ci))
                return new Validacion(false, "El número de carnet es obligatorio");

            if (bioquimico.Ci.Contains(' '))
                return new Validacion(false, "El número de carnet no debe contener espacios");

            if (!Regex.IsMatch(bioquimico.Ci, @"^\d{5,10}(-\d+[A-Za-z])?$"))
                return new Validacion(false, "El CI debe tener de 5 a 10 dígitos y el complemento, si existe, debe seguir el formato 1234567-1A");

            if (string.IsNullOrWhiteSpace(bioquimico.CiExtencion))
                return new Validacion(false, "La extensión del CI es obligatoria");

            if (!ExtensionesValidas.Contains(bioquimico.CiExtencion))
                return new Validacion(false, "La extensión del CI no es válida");

            if (string.IsNullOrWhiteSpace(bioquimico.Telefono))
                return new Validacion(false, "El teléfono es obligatorio");

            if (!Regex.IsMatch(bioquimico.Telefono, @"^\d{8}$"))
                return new Validacion(false, "El teléfono debe tener exactamente 8 dígitos y solo números");

            return new Validacion(true);
        }

        private static bool EsTextoValido(string valor, int minimo, int maximo)
        {
            return !string.IsNullOrWhiteSpace(valor)
                && valor.Length >= minimo
                && valor.Length <= maximo
                && Regex.IsMatch(valor, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");
        }
    }
}
