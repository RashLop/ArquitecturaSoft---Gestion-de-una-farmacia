    using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IConfiguration configuration)
        {
            _smtpSettings = configuration
                .GetSection("SmtpSettings")
                .Get<SmtpSettings>() ?? new SmtpSettings();
        }

        public Validacion EnviarCorreoActivacionCuenta(
            string emailDestino,
            string nombres,
            string userName,
            string passwordTemporal,
            string enlaceActivacion)
        {
            emailDestino = emailDestino?.Trim() ?? string.Empty;
            nombres = nombres?.Trim() ?? string.Empty;
            userName = userName?.Trim() ?? string.Empty;
            passwordTemporal = passwordTemporal?.Trim() ?? string.Empty;
            enlaceActivacion = enlaceActivacion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(emailDestino))
                return Validacion.Fail("El correo destino es obligatorio.");

            if (string.IsNullOrWhiteSpace(userName))
                return Validacion.Fail("El nombre de usuario es obligatorio para el correo.");

            if (string.IsNullOrWhiteSpace(passwordTemporal))
                return Validacion.Fail("La contraseña temporal es obligatoria para el correo.");

            if (string.IsNullOrWhiteSpace(enlaceActivacion))
                return Validacion.Fail("El enlace de activación es obligatorio.");

            try
            {
                string asunto = "Activación de cuenta - Farmacia VitalCare";
                string cuerpoHtml = ConstruirHtmlActivacionCuenta(
                    nombres,
                    userName,
                    passwordTemporal,
                    enlaceActivacion
                );

                using MailMessage message = new MailMessage();
                message.From = new MailAddress(
                    _smtpSettings.RemitenteEmail,
                    _smtpSettings.RemitenteNombre
                );
                message.To.Add(emailDestino);
                message.Subject = asunto;
                message.Body = cuerpoHtml;
                message.IsBodyHtml = true;

                using SmtpClient client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port);
                client.Credentials = new NetworkCredential(
                    _smtpSettings.RemitenteEmail,
                    _smtpSettings.Password
                );
                client.EnableSsl = _smtpSettings.UseSsl;

                client.Send(message);

                return Validacion.Ok();
            }
            catch (Exception ex)
            {
                return Validacion.Fail($"No se pudo enviar el correo electrónico. Detalle: {ex.Message}");
            }
        }

        private string ConstruirHtmlActivacionCuenta(
            string nombres,
            string userName,
            string passwordTemporal,
            string enlaceActivacion)
        {
            string saludo = string.IsNullOrWhiteSpace(nombres) ? "Estimado usuario" : $"Estimado/a {nombres}";

            return $@"
<html>
<head>
    <meta charset='UTF-8'>
</head>
<body style='font-family: Arial, sans-serif; color: #333;'>
    <h2>Activación de cuenta</h2>
    <p>{saludo},</p>
    <p>Tu cuenta ha sido registrada correctamente en el sistema.</p>
    <p><strong>Usuario:</strong> {userName}</p>
    <p><strong>Contraseña temporal:</strong> {passwordTemporal}</p>
    <p>Por seguridad, debes confirmar tu identidad y cambiar tu contraseña ingresando al siguiente enlace:</p>
    <p>
        <a href='{enlaceActivacion}'>Activar cuenta y cambiar contraseña</a>
    </p>
    <p>Este enlace expirará según la política definida por el sistema.</p>
    <p>Atentamente,<br/>Farmacia VitalCare</p>
</body>
</html>";
        }
    }
}