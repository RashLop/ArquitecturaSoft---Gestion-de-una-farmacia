using System.Text.RegularExpressions;
//using ProyectoArqSoft.Application.Interfaces;
//using ProyectoArqSoft.Domain.Model.Clasificacion;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;

//namespace ProyectoArqSoft.Domain.Validators
namespace ProyectoArqSoft.Validaciones
{
    public class ClasificacionValidacion : IValidacion<Clasificacion>
    {
        public Validacion Validar(Clasificacion clasificacion)
        {
            return ValidarNombre(clasificacion.Nombre)
                ?? Validacion.Ok();
        }

        private Validacion? ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Validacion.Fail("El nombre de la clasificación es obligatorio.");

            nombre = nombre.Trim();

            if (nombre.Length < 3 || nombre.Length > 45)
                return Validacion.Fail("El nombre debe tener entre 3 y 45 caracteres.");

            string patron = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$";
            if (!Regex.IsMatch(nombre, patron))
                return Validacion.Fail("El nombre contiene caracteres inválidos.");

            if (Regex.IsMatch(nombre, @"^(.)\1+$"))
                return Validacion.Fail("El nombre no puede estar compuesto por un único carácter repetido.");

            return null;
        }
    }
}