using System.Text.RegularExpressions;
using ProyectoArqSoft.Helpers;

namespace ProyectoArqSoft.Validaciones
{
    public class BioquimicoBusquedaValidacion : IValidacion<string>
    {
        public Validacion Validar(string filtro)
        {
            var validacionGeneral = FiltroHelper.ValidarFiltro(filtro);

            if (validacionGeneral.IsFailure)
                return validacionGeneral;

            if (string.IsNullOrWhiteSpace(filtro))
                return Validacion.Ok();

            filtro = FiltroHelper.LimpiarFiltro(filtro);

            string patronValido = @"^([a-zA-Z\s]+|\d+|\d+-\d[A-Z])$";

            if (!Regex.IsMatch(filtro, patronValido))
                return Validacion.Fail("Criterio inválido.");

            return Validacion.Ok();
        }
    }
}