using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public interface IUsuarioTokenService
    {
        Validacion GenerarToken(UsuarioTokenGeneracionDto dto, out string tokenPlano);
        UsuarioToken? ValidarToken(string tokenPlano, string tipoToken);
        Validacion MarcarComoUsado(int idUsuarioToken);
        Validacion RevocarTokensActivos(int idUsuario, string tipoToken);
        Validacion EliminarTokensObsoletos(int dias);
    }
}