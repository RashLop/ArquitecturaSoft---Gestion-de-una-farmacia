using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.FactoryProducts
{
    public interface IUsuarioTokenRepository : IRepository<UsuarioToken>
    {
        UsuarioToken? GetByTokenHash(string tokenHash);
        UsuarioToken? GetTokenActivo(string tokenHash, string tipoToken);
        int MarcarComoUsado(int idUsuarioToken);
        int RevocarTokensActivos(int idUsuario, string tipoToken);
    }
}