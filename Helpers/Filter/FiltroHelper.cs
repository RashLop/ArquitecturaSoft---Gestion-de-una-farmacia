using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Helpers
{
    public static class FiltroHelper
    {
        public static string LimpiarFiltro(string? filtro)
        {
            return StringHelper.LimpiarEspacios(filtro);
        }

        public static Validacion ValidarFiltro(string filtro, int minimoCaracteres = 3)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return new Validacion(true);

            if (filtro.Length < minimoCaracteres)
                return new Validacion(false,$"El criterio debe tener al menos {minimoCaracteres} caracteres");

            if (!filtro.All(c => char.IsLetterOrDigit(c) || c == ' '))
                return new Validacion(false, "Criterio inválido");

            return new Validacion(true);
        }

        public static string[] ObtenerPartes(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Array.Empty<string>();

            return filtro.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}