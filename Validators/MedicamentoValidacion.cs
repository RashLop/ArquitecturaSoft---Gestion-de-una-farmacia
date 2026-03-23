using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    public class MedicamentoValidacion : IValidacion<Medicamento>
    {
        public Validacion Validar(Medicamento medicamento)
        {
            if (string.IsNullOrWhiteSpace(medicamento.Nombre))
                return new Validacion(false, "El nombre es obligatorio.");
            if (!EsNombreValido(medicamento.Nombre))
                return new Validacion(false, "El nombre contiene caracteres inválidos o no tiene un formato correcto.");

            if (string.IsNullOrWhiteSpace(medicamento.Presentacion))
                return new Validacion(false, "La presentación es obligatoria.");

            if (string.IsNullOrWhiteSpace(medicamento.Clasificacion))
                return new Validacion(false, "La clasificación es obligatoria.");

            if (string.IsNullOrWhiteSpace(medicamento.Concentracion))
                return new Validacion(false, "La concentración es obligatoria.");

            if (!EsConcentracionValida(medicamento.Concentracion))
                return new Validacion(false, "La concentración no tiene un formato válido. Ejemplos: 500 mg, 250 mg/5ml, 0.9 %.");

            if (medicamento.Precio <= 0)
                return new Validacion(false, "El precio debe ser mayor a 0 Bs.");

            if (medicamento.Stock < 0)
                return new Validacion(false, "El stock no puede ser negativo.");

            if (medicamento.Stock > 100000)
                return new Validacion(false, "El stock no puede ser mayor a 100000 items.");
            
            if(medicamento.Precio > 1000)
                return new Validacion(false, "El precio no puede ser mayor a 1000 Bs.");

            return new Validacion(true, string.Empty);
        }

        private bool EsConcentracionValida(string concentracion)
        {
            string patron = @"^\d+(\.\d+)?\s?(mg|g|mcg|ml|%)\s*(\/\s*(\d+(\.\d+)?)?\s?(ml|l))?$";

            return Regex.IsMatch(concentracion.Trim(), patron, RegexOptions.IgnoreCase);
        }
        private bool EsNombreValido(string nombre)
            {
                nombre = nombre.Trim();

                string patron = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$";
                if (!Regex.IsMatch(nombre, patron))
                    return false;
                if (nombre.Length < 3)
                    return false;
                if (nombre.Length > 100)
                    return false;
                if (Regex.IsMatch(nombre, @"^(.)\1+$"))
                    return false;

                return true;
            }
    }
}