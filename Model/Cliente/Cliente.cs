namespace ProyectoArqSoft.Models;

public class Cliente
{
    public int IdCliente { get; set; }
    public string Tipo_Cliente { get; set; }
    public string Nombre { get; set; }
    public string Apellido_Materno { get; set; }
    public string Apellido_Paterno { get; set; }
    public string Ci_Extencion { get; set; }
    public string Ci { get; set; }
    public DateTime Fecha_De_Nacimiento { get; set; }
    public string Telefono { get; set; }
    public short Estado { get; set; }
    public DateTime Fecha_Registro { get; set; }
    public DateTime? Ultima_Actualizacion { get; set; }

    // Propiedades calculadas para facilitar el uso
    public string NombreCompleto => $"{Nombre} {Apellido_Paterno} {Apellido_Materno}".Trim();
    public string CiCompleto => $"{Ci} {Ci_Extencion}".Trim();

    // Edad calculada
    public int Edad
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - Fecha_De_Nacimiento.Year;
            if (Fecha_De_Nacimiento.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}