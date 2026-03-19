using System.Text.RegularExpressions;

namespace ProyectoArqSoft.Validaciones
{
    public class BioquimicoBusquedaValidacion : IValidacion<string>
    {
        public Validacion Validar(string filtro)
        {
            // Si no hay filtro, es válido (lista todos)
            if (string.IsNullOrWhiteSpace(filtro)) 
                return new Validacion(true, string.Empty);

            // EXPLICACIÓN DEL REGEX:
            // ^[a-zA-Z\s]+$                -> Solo letras y espacios (Nombres)
            // |                            -> O
            // ^\d+$                        -> Solo números (CI Simple)
            // |                            -> O
            // ^\d+-\d[A-Z]$                -> Formato Números-NúmeroLetra (CI con complemento ej: 9132482-1A)
            
            string patronValido = @"^([a-zA-Z\s]+|\d+|\d+-\d[A-Z])$";

            if (!Regex.IsMatch(filtro.Trim(), patronValido))
            {
                // Si pone un punto (12.) o caracteres no permitidos
                return new Validacion(false, "Criterio inválido");
            }

            // Validación de longitud máxima
            if (filtro.Length > 20)
                return new Validacion(false, "Criterio inválido");

            return new Validacion(true, string.Empty);
        }
    }
}