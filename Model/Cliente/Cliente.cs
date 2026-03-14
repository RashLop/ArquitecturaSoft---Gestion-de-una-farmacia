namespace ProyectoArqSoft.Models;

public class Cliente
{
    public int IdCliente { get; set; }
    public string TipoCliente { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string CiExtencion { get; set; } = string.Empty;
    public string Ci { get; set; } = string.Empty;
    public DateTime FechaDeNacimiento { get; set; }
    public string Telefono { get; set; } = string.Empty;
    public short Estado { get; set; } = 1;
    public DateTime FechaRegistro { get; set; }
    public DateTime UltimaActualizacion { get; set; }

    public Cliente()
    {
    }

    public Cliente(
        string tipoCliente,
        string nombre,
        string apellidoMaterno,
        string apellidoPaterno,
        string ciExtencion,
        string ci,
        DateTime fechaDeNacimiento,
        string telefono)
    {
        TipoCliente = tipoCliente;
        Nombre = nombre;
        ApellidoMaterno = apellidoMaterno;
        ApellidoPaterno = apellidoPaterno;
        CiExtencion = ciExtencion;
        Ci = ci;
        FechaDeNacimiento = fechaDeNacimiento;
        Telefono = telefono;
    }
}
