using System.Text.RegularExpressions;

namespace ProyectoArqSoft.Validaciones
{
    public class BioquimicoBusquedaValidacion : IValidacion<string>
    {
        public Validacion Validar(string filtro)
        {
            
            if (string.IsNullOrWhiteSpace(filtro)) 
                return new Validacion(true, string.Empty);

            
            
            string patronValido = @"^([a-zA-Z\s]+|\d+|\d+-\d[A-Z])$";

            if (!Regex.IsMatch(filtro.Trim(), patronValido))
            {
                
                return new Validacion(false, "Criterio inválido");
            }

           
            if (filtro.Length > 20)
                return new Validacion(false, "Criterio inválido");

            return new Validacion(true, string.Empty);
        }
    }
}