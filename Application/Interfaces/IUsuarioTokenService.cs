using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Services
{
    public interface IUsuarioTokenService
    {
        string GenerarToken(int idUsuario, string tipoToken, int minutosExpiracion);
        UsuarioToken? ValidarToken(string tokenPlano, string tipoToken);
        int MarcarComoUsado(int idUsuarioToken);
        int RevocarTokensActivos(int idUsuario, string tipoToken);
    }
}