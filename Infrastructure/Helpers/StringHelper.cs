using System.Text.RegularExpressions;

namespace ProyectoArqSoft.Infrastructure.Helpers
{
    public static class StringHelper
    {
        public static string Limpiar(string? texto)
        {
            return texto?.Trim() ?? "";
        }

        public static string LimpiarEspacios(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            return Regex.Replace(texto.Trim(), @"\s+", " ");
        }

        public static string QuitarEspacios(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            return Regex.Replace(texto, @"\s+", "");
        }

        public static string LimpiarTexto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            // Trim + quitar espacios múltiples
            texto = Regex.Replace(texto.Trim(), @"\s+", " ");

            return texto;
        }

        public static string LimpiarTextoMayus(string? texto)
        {
            return LimpiarTexto(texto).ToUpper();
        }

        public static string LimpiarTextoMinus(string? texto)
        {
            return LimpiarTexto(texto).ToLower();
        }

        public static string SoloNumeros(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            return Regex.Replace(texto, @"\D", "");
        }

        public static string LimpiarCI(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            return Regex.Replace(texto.Trim(), @"\s+", "").ToUpper();
        }

        public static bool NombrePareceFragmentado(string? nombres)
            {
                nombres = LimpiarTexto(nombres);

                if (string.IsNullOrWhiteSpace(nombres))
                    return true;

                string[] partes = nombres.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (partes.Length == 0)
                    return true;

                int palabrasDeUnCaracter = partes.Count(p => p.Length == 1);
                int palabrasDeDosCaracteres = partes.Count(p => p.Length == 2);
                int palabrasCortas = partes.Count(p => p.Length <= 2);

                if (palabrasDeUnCaracter >= 2)
                    return true;


                if (partes.Length >= 4 && palabrasCortas >= 3)
                    return true;

                if (partes.Length == 2)
                {
                    if (partes[0].Length >= 3 && partes[1].Length <= 2)
                        return true;

                    if (partes[0].Length <= 2 && partes[1].Length >= 3)
                        return true;
                }

                if (partes.Length >= 3 && palabrasCortas >= 2)
                    return true;

                return false;
            }

        public static bool ApellidoPareceFragmentado(string? apellido)
        {
            apellido = LimpiarTexto(apellido);

            if (string.IsNullOrWhiteSpace(apellido))
                return true;

            string[] partes = apellido.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 0)
                return true;

            HashSet<string> conectoresValidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "de", "del", "la", "las", "los", "san", "santa", "van", "von"
            };

            // Casos tipo: "d e la fuente", "de l a fuente"
            if (partes.Any(p => p.Length == 1))
                return true;

            // Casos tipo: "fuen te", "cr uz"
            if (partes.Length == 2)
            {
                bool primeraEsConector = conectoresValidos.Contains(partes[0]);
                bool segundaEsConector = conectoresValidos.Contains(partes[1]);

                if (!primeraEsConector && !segundaEsConector)
                {
                    if (partes[0].Length >= 3 && partes[1].Length <= 2)
                        return true;

                    if (partes[0].Length <= 2 && partes[1].Length >= 3)
                        return true;
                }
            }

            // Casos tipo: "de l a fuente"
            if (partes.Length >= 3)
            {
                int cortasNoValidas = partes.Count(p => p.Length <= 2 && !conectoresValidos.Contains(p));
                if (cortasNoValidas >= 1)
                    return true;
            }

            return false;
        }

        
    }
}
