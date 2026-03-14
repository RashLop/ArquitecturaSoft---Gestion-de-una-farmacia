namespace ProyectoArqSoft.Models;

public class Bioquimico
{
    public int IdBioquimico { get; set; }
    public string Nombres { get; set; }
    public string Apellido_Materno { get; set; }
    public string Apellido_Paterno { get; set; }
    public string Ci { get; set; }
    public string Ci_Extencion { get; set; }
    public string Telefono { get; set; }
    public bool Activo { get; set; }
    public DateTime Fecha_Registro { get; set; }
    public DateTime? Ultima_Actualizacion { get; set; }

    // Propiedades calculadas
    public string NombreCompleto => $"{Nombres} {Apellido_Paterno} {Apellido_Materno}".Trim();
    public string CiCompleto => $"{Ci} {Ci_Extencion}".Trim();

    public Bioquimico() { }

    public Bioquimico(string nombres, string apellido_materno, string apellido_paterno,
                      string ci, string ci_extencion, string telefono)
    {
        Nombres = nombres?.Trim() ?? "";
        Apellido_Materno = apellido_materno?.Trim() ?? "";
        Apellido_Paterno = apellido_paterno?.Trim() ?? "";
        Ci = ci?.Trim() ?? "";
        Ci_Extencion = ci_extencion?.Trim().ToUpper() ?? "LP";
        Telefono = telefono?.Trim() ?? "";
        Activo = true;
        Fecha_Registro = DateTime.Now;
    }
}