using ProyectoArqSoft.Domain.Models;

namespace ProyectoArqSoft.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerarToken(Usuario usuario, out int expiraEn);
    }
}