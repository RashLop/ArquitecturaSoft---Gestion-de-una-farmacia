using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public interface IEmailService
    {
        Validacion EnviarCorreoActivacionCuenta(
            string emailDestino,
            string nombres,
            string userName,
            string passwordTemporal,
            string enlaceActivacion
        );
    }
}