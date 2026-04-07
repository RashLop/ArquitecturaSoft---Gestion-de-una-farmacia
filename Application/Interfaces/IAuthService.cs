using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Domain.Validators;

namespace ProyectoArqSoft.Services
{
    public interface IAuthService
    {
        Result IniciarSesion(UsuarioLoginRequestDto dto, out UsuarioLoginResponseDto? respuesta);
    }
}