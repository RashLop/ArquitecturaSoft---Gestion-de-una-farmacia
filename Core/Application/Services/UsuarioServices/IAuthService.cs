using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public interface IAuthService
    {
        Validacion IniciarSesion(UsuarioLoginRequestDto dto, out UsuarioLoginResponseDto? respuesta);
    }
}