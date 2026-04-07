using System.Text.RegularExpressions;
using ProyectoArqSoft.Infrastructure.Helpers;
using ProyectoArqSoft.Application.Interfaces;

namespace ProyectoArqSoft.Domain.Validators
{
    public class BioquimicoBusquedaValidacion : IResult<string>
    {
        public Result Validar(string filtro)
        {
            var validacionGeneral = FiltroHelper.ValidarFiltro(filtro);

            if (validacionGeneral.IsFailure)
                return validacionGeneral;

            if (string.IsNullOrWhiteSpace(filtro))
                return Result.Ok();

            filtro = FiltroHelper.LimpiarFiltro(filtro);

            string patronValido = @"^([a-zA-Z\s]+|\d+|\d+-\d[A-Z])$";

            if (!Regex.IsMatch(filtro, patronValido))
                return Result.Fail("Criterio inválido.");

            return Result.Ok();
        }
    }
}