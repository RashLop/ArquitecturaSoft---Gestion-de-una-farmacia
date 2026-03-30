namespace ProyectoArqSoft.DTO
{
    public class UsuarioActualizacionDto
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}