namespace ProyectoArqSoft.Domain.DTOs
{
    public class UsuarioActualizacionDto
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Ci { get; set; } = string.Empty;
        public string CiExtencion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public sbyte Activo { get; set; } = 1;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime? UltimaActualizacion { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public sbyte MustChangePassword { get; set; } = 1;
    }
}
