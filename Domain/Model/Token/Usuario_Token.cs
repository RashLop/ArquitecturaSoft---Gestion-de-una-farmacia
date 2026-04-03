
namespace ProyectoArqSoft.Models
{
    public class UsuarioToken
    {
        public int IdUsuarioToken { get; set; }
        public int UsuarioIdUsuario { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public string TipoToken { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public sbyte Revocado { get; set; } = 0;
        public sbyte Usado { get; set; } = 0;

        public UsuarioToken() { }
        public UsuarioToken(int usuarioIdUsuario, string tokenHash, string tipoToken, DateTime fechaCreacion, DateTime fechaExpiracion)
        {
            UsuarioIdUsuario = usuarioIdUsuario;
            TokenHash = tokenHash;
            TipoToken = tipoToken;
            FechaCreacion = fechaCreacion;
            FechaExpiracion = fechaExpiracion;
            Revocado = 0;
            Usado = 0;
        }
    }
}