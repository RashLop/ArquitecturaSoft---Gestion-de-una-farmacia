using System.Text.RegularExpressions;
//using ProyectoArqSoft.Application.Interfaces;
//using ProyectoArqSoft.Domain.Model.Clasificacion;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;

//namespace ProyectoArqSoft.Domain.Validators
namespace ProyectoArqSoft.Validaciones
{
    public class ClasificacionValidacion : IResult<Clasificacion>
    {
        public Result Validar(Clasificacion clasificacion)
        {
            return ValidarNombre(clasificacion.Nombre)
                ?? Result.Ok();
        }

        private Result? ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Result.Fail("El nombre de la clasificación es obligatorio.");

            nombre = nombre.Trim();

            if (nombre.Length < 3 || nombre.Length > 45)
                return Result.Fail("El nombre debe tener entre 3 y 45 caracteres.");

            string patron = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$";
            if (!Regex.IsMatch(nombre, patron))
                return Result.Fail("El nombre contiene caracteres inválidos.");

            if (Regex.IsMatch(nombre, @"^(.)\1+$"))
                return Result.Fail("El nombre no puede estar compuesto por un único carácter repetido.");

            return null;
        }
    }
}