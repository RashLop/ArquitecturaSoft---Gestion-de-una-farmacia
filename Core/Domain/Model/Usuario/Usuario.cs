namespace ProyectoArqSoft.Models ;


public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public sbyte MustChangePassword { get; set; } = 1;
        public sbyte IsActive { get; set; } = 1;
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimaActualizacion { get; set; }
        public int? BioquimicoIdBioquimico { get; set; }
        // Propiedad de navegación opcional
        public Bioquimico? Bioquimico { get; set; }

        public Usuario() { }

        public Usuario(
            string email,
            string userName,
            string passwordHash,
            string role,
            sbyte mustChangePassword,
            sbyte isActive,
            int bioquimicoIdBioquimico)
        {
            Email = email;
            UserName = userName;
            PasswordHash = passwordHash;
            Role = role;
            MustChangePassword = mustChangePassword;
            IsActive = isActive;
            BioquimicoIdBioquimico = bioquimicoIdBioquimico;
        }
    }