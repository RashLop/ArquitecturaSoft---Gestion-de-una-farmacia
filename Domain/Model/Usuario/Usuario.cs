using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoArqSoft.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string Ci { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public sbyte Activo { get; set; } = 1;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime? UltimaActualizacion { get; set; }
        public string CiExtencion { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public sbyte MustChangePassword { get; set; } = 1;
        public Usuario() { }

        public Usuario(string nombres, string apellidoPaterno, string apellidoMaterno, 
        string ci, string ciExtencion, string email, string userName, string passwordHash, string role)
        {
            Nombres = nombres;
            ApellidoPaterno = apellidoPaterno;
            ApellidoMaterno = apellidoMaterno;
            Ci = ci;
            CiExtencion = ciExtencion;
            Email = email;
            UserName = userName;
            PasswordHash = passwordHash;
            Role = role;
            Activo = 1;
            MustChangePassword = 1;
            FechaRegistro = DateTime.Now;
        }
    }
}