namespace ProyectoArqSoft.Application.Helpers
{
    public class NumeroATextoConverter
    {
        private static readonly string[] Unidades = { "", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve", "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };
        private static readonly string[] Decenas = { "", "", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        private static readonly string[] Centenas = { "", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };

        public static string ConvertirDecimalATexto(decimal cantidad)
        {
            string texto = "";

            if (cantidad == 0)
                return "cero";

            if (cantidad >= 1000)
            {
                texto += ConvertirDecimalATexto((int)(cantidad / 1000)) + " mil ";
                cantidad %= 1000;
            }

            if (cantidad >= 100)
            {
                texto += Centenas[(int)(cantidad / 100)] + " ";
                cantidad %= 100;
            }

            if (cantidad >= 20)
            {
                texto += Decenas[(int)(cantidad / 10)] + " ";
                cantidad %= 10;
            }

            if (cantidad > 0)
            {
                texto += Unidades[(int)cantidad] + " ";
            }

            return texto.Trim();
        }
    }
}