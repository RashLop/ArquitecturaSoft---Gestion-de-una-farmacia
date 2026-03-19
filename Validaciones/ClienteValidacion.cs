using System.Text.RegularExpressions;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Helpers;

namespace ProyectoArqSoft.Validaciones
{
    public class ClienteValidacion : IValidacion<Cliente>
    {
        public Validacion Validar(Cliente cliente)
        {
            // Limpiar campos usando StringHelper
            string nit = StringHelper.QuitarEspacios(cliente.Nit);
            string razonSocial = StringHelper.LimpiarEspacios(cliente.RazonSocial);
            string? correo = string.IsNullOrWhiteSpace(cliente.CorreoElectronico) ? null : StringHelper.QuitarEspacios(cliente.CorreoElectronico);

            if (string.IsNullOrWhiteSpace(nit))
                return new Validacion(false, "El NIT es obligatorio");

            if (!Regex.IsMatch(nit, @"^\d+$"))
                return new Validacion(false, "El NIT debe contener solo números");

            if (nit.Length < 5 || nit.Length > 15)
                return new Validacion(false, "El NIT debe tener entre 5 y 15 dígitos");

            if (string.IsNullOrWhiteSpace(razonSocial))
                return new Validacion(false, "La razón social es obligatoria");

            if (razonSocial.Length < 3 || razonSocial.Length > 100)
                return new Validacion(false, "La razón social debe tener entre 3 y 100 caracteres");

            if (!string.IsNullOrWhiteSpace(correo))
            {
                if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return new Validacion(false, "El formato del correo electrónico no es válido");
            }

            return new Validacion(true);
        }
    }
}