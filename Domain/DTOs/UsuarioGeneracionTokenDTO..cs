namespace ProyectoArqSoft.Domain.DTOs
{
    public class UsuarioTokenGeneracionDto
    {
        public int IdUsuario { get; set; }
        public string TipoToken { get; set; } = string.Empty;
        public int MinutosExpiracion { get; set; }
    }
}