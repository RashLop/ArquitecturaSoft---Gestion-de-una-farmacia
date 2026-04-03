using System;
using Microsoft.AspNetCore.SignalR;

namespace ProyectoArqSoft.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string Ci { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; }
    public DateTime? UltimaActualizacion { get; set; }
    public string CiExtension { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool MustChangePassword { get; set; } = true;

    // Auditoría
    public string UsuarioCreacion { get; set; } = string.Empty;
    public DateTime? FechaCreacion { get; set; }
    public string UsuarioEdicion { get; set; } = string.Empty;
    public DateTime? FechaEdicion { get; set; }
    public string UsuarioBaja { get; set; } = string.Empty;
    public DateTime? FechaBaja { get; set; }

    public Usuario()
    {
    }

    public Usuario(string nombres, string apellidoMaterno, string apellidoPaterno, string ci,
                   string ciExtension, string telefono, string email, string role)
    {
        Nombres = nombres;
        ApellidoMaterno = apellidoMaterno;
        ApellidoPaterno = apellidoPaterno;
        Ci = ci;
        CiExtension = ciExtension;
        Telefono = telefono;
        Email = email;
        Role = role;
    }
}