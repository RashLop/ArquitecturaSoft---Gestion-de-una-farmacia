using ProyectoArqSoft.DTO;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public class UsuarioTokenService : IUsuarioTokenService
    {
        private readonly IUsuarioTokenRepository _repository;
        private readonly IValidacion<UsuarioTokenGeneracionDto> _generacionValidador;

        public UsuarioTokenService(
            IUsuarioTokenRepository repository,
            IValidacion<UsuarioTokenGeneracionDto> generacionValidador)
        {
            _repository = repository;
            _generacionValidador = generacionValidador;
        }

        public Validacion GenerarToken(UsuarioTokenGeneracionDto dto, out string tokenPlano)
        {
            tokenPlano = string.Empty;

            Validacion validacionEntrada = _generacionValidador.Validar(dto);
            if (!validacionEntrada.IsSuccess)
                return validacionEntrada;

            string tipoToken = dto.TipoToken.Trim();

            _repository.EliminarTokensObsoletos(7);
            _repository.RevocarTokensActivos(dto.IdUsuario, tipoToken);

            tokenPlano = TokenHelper.GenerarTokenPlano();
            string tokenHash = TokenHelper.GenerarTokenHash(tokenPlano);
            DateTime fechaExpiracion = TokenHelper.GenerarFechaExpiracion(dto.MinutosExpiracion);

            UsuarioToken token = new UsuarioToken(dto.IdUsuario, tokenHash, tipoToken, fechaExpiracion);

            int filasAfectadas = _repository.Insert(token);

            if (filasAfectadas <= 0)
            {
                tokenPlano = string.Empty;
                return Validacion.Fail("No se pudo generar el token.");
            }

            return Validacion.Ok();
        }

        public UsuarioToken? ValidarToken(string tokenPlano, string tipoToken)
        {
            tokenPlano = tokenPlano?.Trim() ?? string.Empty;
            tipoToken = tipoToken?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(tokenPlano))
                return null;

            if (string.IsNullOrWhiteSpace(tipoToken))
                return null;

            string tokenHash = TokenHelper.GenerarTokenHash(tokenPlano);

            return _repository.GetTokenActivo(tokenHash, tipoToken);
        }

        public Validacion MarcarComoUsado(int idUsuarioToken)
        {
            if (idUsuarioToken <= 0)
                return Validacion.Fail("El id del token debe ser mayor a cero.");

            int filasAfectadas = _repository.MarcarComoUsado(idUsuarioToken);

            if (filasAfectadas <= 0)
                return Validacion.Fail("No se pudo marcar el token como usado.");

            return Validacion.Ok();
        }

        public Validacion RevocarTokensActivos(int idUsuario, string tipoToken)
        {
            if (idUsuario <= 0)
                return Validacion.Fail("El id del usuario debe ser mayor a cero.");

            tipoToken = tipoToken?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(tipoToken))
                return Validacion.Fail("El tipo de token es obligatorio.");

            _repository.RevocarTokensActivos(idUsuario, tipoToken);

            return Validacion.Ok();
        }

        public Validacion EliminarTokensObsoletos(int dias)
        {
            if (dias <= 0)
                return Validacion.Fail("La cantidad de días debe ser mayor a cero.");

            _repository.EliminarTokensObsoletos(dias);

            return Validacion.Ok();
        }
    }
}