using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    public class MedicamentoValidacion : IValidacion<Medicamento>
    {
        public Validacion Validar(Medicamento medicamento)
        {
            return ValidarNombre(medicamento.Nombre)
                ?? ValidarPresentacion(medicamento.Presentacion)
                ?? ValidarClasificacion(medicamento.Clasificacion)
                ?? ValidarConcentracion(medicamento.Concentracion)
                ?? ValidarPrecio(medicamento.Precio)
                ?? ValidarStock(medicamento.Stock)
                ?? Validacion.Ok();
        }

        private Validacion? ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Validacion.Fail("El nombre es obligatorio.");

            if (!EsNombreValido(nombre))
                return Validacion.Fail("El nombre contiene caracteres inválidos o no tiene un formato correcto.");

            return null;
        }

        private Validacion? ValidarPresentacion(string presentacion)
        {
            return string.IsNullOrWhiteSpace(presentacion)
                ? Validacion.Fail("La presentación es obligatoria.")
                : null;
        }

        private Validacion? ValidarClasificacion(string clasificacion)
        {
            return string.IsNullOrWhiteSpace(clasificacion)
                ? Validacion.Fail("La clasificación es obligatoria.")
                : null;
        }

        private Validacion? ValidarConcentracion(string concentracion)
        {
            if (string.IsNullOrWhiteSpace(concentracion))
                return Validacion.Fail("La concentración es obligatoria.");

            if (!EsConcentracionValida(concentracion))
                return Validacion.Fail("La concentración no tiene un formato válido. Ejemplos: 500 mg, 250 mg/5ml, 0.9 %.");

            return null;
        }

        private Validacion? ValidarPrecio(decimal precio)
        {
            if (precio <= 0)
                return Validacion.Fail("El precio debe ser mayor a 0 Bs.");

            if (precio > 1000)
                return Validacion.Fail("El precio no puede ser mayor a 1000 Bs.");

            return null;
        }

        private Validacion? ValidarStock(int stock)
        {
            if (stock < 0)
                return Validacion.Fail("El stock no puede ser negativo.");

            if (stock > 100000)
                return Validacion.Fail("El stock no puede ser mayor a 100000 items.");

            return null;
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

            if (nombre.Length < 3 || nombre.Length > 100)
                return false;

            if (Regex.IsMatch(nombre, @"^(.)\1+$"))
                return false;

            return true;
        }
    }
}