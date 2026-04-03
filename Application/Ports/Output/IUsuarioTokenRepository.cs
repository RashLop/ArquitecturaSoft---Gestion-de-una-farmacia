using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.FactoryProducts
{
    public interface IUsuarioTokenRepository
    {
        int Insert(UsuarioToken token);
        UsuarioToken? GetTokenActivo(string tokenHash, string tipoToken);
        int MarcarComoUsado(int idUsuarioToken);
        int RevocarTokensActivos(int idUsuario, string tipoToken);
    }
}