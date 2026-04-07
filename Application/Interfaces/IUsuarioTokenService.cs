using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public interface IUsuarioTokenService
    {
        Result GenerarToken(UsuarioTokenGeneracionDto dto, out string tokenPlano);
        UsuarioToken? ValidarToken(string tokenPlano, string tipoToken);
        Result MarcarComoUsado(int idUsuarioToken);
        Result RevocarTokensActivos(int idUsuario, string tipoToken);
        Result EliminarTokensObsoletos(int dias);
    }
}