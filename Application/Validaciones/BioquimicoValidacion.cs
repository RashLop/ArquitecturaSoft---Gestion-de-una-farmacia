using ProyectoArqSoft.Domain;

namespace ProyectoArqSoft.Application.Validaciones
{
    public static class BioquimicoValidacion
    {
        public static Validacion ValidarCriterioBusqueda(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro)) return new Validacion(true);

            
            if (filtro.Any(char.IsDigit) && filtro.Length > 15)
                return new Validacion(false, "Criterio inválido");

            return new Validacion(true);
        }
    }
}