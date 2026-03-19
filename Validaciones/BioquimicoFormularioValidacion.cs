using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    // --- 1. VALIDACIÓN PARA EL FORMULARIO (CREAR/EDITAR) ---
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
                return new Validacion(false, "El CI debe tener de 5 a 10 dígitos y el formato correcto (ej: 1234567 o 1234567-1A)");

            if (string.IsNullOrWhiteSpace(bioquimico.CiExtencion))
                return new Validacion(false, "La extensión del CI es obligatoria");

            if (!ExtensionesValidas.Contains(bioquimico.CiExtencion))
                return new Validacion(false, "La extensión del CI no es válida");

            if (string.IsNullOrWhiteSpace(bioquimico.Telefono))
                return new Validacion(false, "El teléfono es obligatorio");

            if (!Regex.IsMatch(bioquimico.Telefono, @"^\d{8}$"))
                return new Validacion(false, "El teléfono debe tener exactamente 8 dígitos");

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

    // --- 2. VALIDACIÓN PARA EL BUSCADOR (FILTRO) ---
    public class BioquimicoBusquedasValidacion : IValidacion<string>
    {
        public Validacion Validar(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro)) 
                return new Validacion(true);

            // Letras, números o formato CI con complemento
            string patronValido = @"^([a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+|\d+|\d+-\d[A-Z])$";

            if (!Regex.IsMatch(filtro.Trim(), patronValido))
                return new Validacion(false, "Criterio de búsqueda inválido");

            if (filtro.Length > 20)
                return new Validacion(false, "El criterio es demasiado largo");

            return new Validacion(true);
        }
    }
}