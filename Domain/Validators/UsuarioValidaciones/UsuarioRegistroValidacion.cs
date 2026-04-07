using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Application.Interfaces;

namespace ProyectoArqSoft.Domain.Validators
{
    public class UsuarioRegistroValidacion : UsuarioValidacionBase, IResult<UsuarioRegistroDto>
    {
        public Result Validar(UsuarioRegistroDto dto)
        {
            if (dto == null)
                return Result.Fail("Los datos del usuario no pueden ser nulos.");

            string nombres = dto.Nombres?.Trim() ?? string.Empty;
            string apellidoPaterno = dto.ApellidoPaterno?.Trim() ?? string.Empty;
            string apellidoMaterno = dto.ApellidoMaterno?.Trim() ?? string.Empty;
            string ci = dto.Ci?.Trim() ?? string.Empty;
            string ciExtencion = dto.CiExtencion?.Trim().ToUpper() ?? string.Empty;
            string telefono = dto.Telefono?.Trim() ?? string.Empty;
            string email = dto.Email?.Trim() ?? string.Empty;

            return
                TextoRequerido(nombres, "Nombres", 45) ??
                TextoRequerido(apellidoPaterno, "Primer apellido", 45) ??
                TextoOpcional(apellidoMaterno, "Segundo apellido", 45) ??
                ValidarCi(ci) ??
                ValidarCiExtencion(ciExtencion) ??
                ValidarTelefono(telefono) ??
                EmailValido(email) ??
                Result.Ok();
        }

        private Result? ValidarCi(string ci)
        {
            return Requerido(ci, "El número de carnet es obligatorio.")
                ?? (ci.Contains(' ') ? Result.Fail("El número de carnet no debe contener espacios.") : null)
                ?? RegexValido(ci, @"^\d{5,10}$", "El CI debe tener entre 5 y 10 dígitos.");
        }

        private Result? ValidarCiExtencion(string ciExtencion)
        {
            return Requerido(ciExtencion, "La extensión del CI es obligatoria.")
                ?? (!ExtensionesValidas.Contains(ciExtencion)
                    ? Result.Fail("La extensión del CI no es válida.")
                    : null);
        }

        private Result? ValidarTelefono(string telefono)
        {
            return Requerido(telefono, "El teléfono es obligatorio.")
                ?? Maximo(telefono, 8, "El teléfono debe tener 8 dígitos.")
                ?? RegexValido(telefono, @"^\d{8}$", "El teléfono debe tener exactamente 8 dígitos numéricos.");
        }
    }
}
