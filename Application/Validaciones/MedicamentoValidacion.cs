using System.Text.RegularExpressions;
using ProyectoArqSoft.Domain.Entities.Medicamento;

namespace ProyectoArqSoft.Application.Validaciones
{
    public class MedicamentoValidacion : IValidacion<Medicamento>
    {
        public Validacion Validar(Medicamento medicamento)
        {
            if (string.IsNullOrWhiteSpace(medicamento.Nombre))
                return new Validacion(false, "El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(medicamento.Presentacion))
                return new Validacion(false, "La presentación es obligatoria.");

            if (string.IsNullOrWhiteSpace(medicamento.Clasificacion))
                return new Validacion(false, "La clasificación es obligatoria.");

            if (string.IsNullOrWhiteSpace(medicamento.Concentracion))
                return new Validacion(false, "La concentración es obligatoria.");

            if (!EsConcentracionValida(medicamento.Concentracion))
                return new Validacion(false, "La concentración no tiene un formato válido. Ejemplos: 500 mg, 250 mg/5ml, 0.9 %.");

            if (medicamento.Precio <= 0)
                return new Validacion(false, "El precio debe ser mayor a 0.");

            if (medicamento.Stock < 0)
                return new Validacion(false, "El stock no puede ser negativo.");

            return new Validacion(true, string.Empty);
        }

        private bool EsConcentracionValida(string concentracion)
        {
            string patron = @"^\d+(\.\d+)?\s?(mg|g|mcg|ml|%)\s*(\/\s*(\d+(\.\d+)?)?\s?(ml|l))?$";

            return Regex.IsMatch(concentracion.Trim(), patron, RegexOptions.IgnoreCase);
        }
    }
}