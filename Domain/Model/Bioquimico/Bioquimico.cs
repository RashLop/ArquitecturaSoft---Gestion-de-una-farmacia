namespace ProyectoArqSoft.Models;

public class Bioquimico
{
    public int IdBioquimico { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string Ci { get; set; } = string.Empty;
    public string CiExtencion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public sbyte Activo { get; set; } = 1;
    
   
    public DateTime FechaRegistro { get; set; }
    public DateTime? UltimaActualizacion { get; set; }

    
    public Bioquimico() { }

    
    public Bioquimico(string nombres, string apellidoPaterno, string apellidoMaterno, 
                      string ci, string ciExtencion, string telefono)
    {
        Nombres = nombres;
        ApellidoPaterno = apellidoPaterno;
        ApellidoMaterno = apellidoMaterno;
        Ci = ci;
        CiExtencion = ciExtencion;
        Telefono = telefono;
        FechaRegistro = DateTime.Now;
        Activo = 1;
    }
}