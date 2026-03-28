using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Helpers
{
    public static class FiltroHelper
    {
        public static string LimpiarFiltro(string? filtro)
        {
            return StringHelper.LimpiarEspacios(filtro);
        }

        public static Validacion ValidarFiltro(string filtro, int minimoCaracteres = 3, int maximoCaracteres = 20)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Validacion.Ok();

            filtro = LimpiarFiltro(filtro);

            if (filtro.Length < minimoCaracteres)
                return Validacion.Fail($"El criterio debe tener al menos {minimoCaracteres} caracteres.");

            if (filtro.Length > maximoCaracteres)
                return Validacion.Fail($"El criterio no puede tener más de {maximoCaracteres} caracteres.");

            if (!filtro.All(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-'))
                return Validacion.Fail("Criterio inválido.");

            return Validacion.Ok();
        }

        public static string[] ObtenerPartes(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Array.Empty<string>();

            return filtro.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}