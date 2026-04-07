
namespace ProyectoArqSoft.Domain.DTOs
{   
    public class UsuarioEdicionDto
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public sbyte Activo { get; set; }
    }
}