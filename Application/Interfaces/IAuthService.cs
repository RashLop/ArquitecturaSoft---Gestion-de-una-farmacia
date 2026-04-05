using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public interface IAuthService
    {
        Result IniciarSesion(UsuarioLoginRequestDto dto, out UsuarioLoginResponseDto? respuesta);
    }
}