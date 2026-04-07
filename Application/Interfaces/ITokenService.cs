using ProyectoArqSoft.Domain.Models;

namespace ProyectoArqSoft.Services
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario, out int expiraEn);
    }
}