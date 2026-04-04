using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public interface IEmailService
    {
        Result EnviarCorreoActivacionCuenta(
            string emailDestino,
            string nombres,
            string userName,
            string passwordTemporal,
            string enlaceActivacion
        );
    }
}