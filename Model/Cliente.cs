namespace ProyectoArqSoft.Models;

public class Cliente
{
    public int IdCliente { get; set; }
    public string TipoCliente { get; set; }
    public string Nombre { get; set; }
    public string Ci { get; set; }
    public int Edad { get; set; }
    public string Telefono { get; set; }

    public Cliente()
    {
    }

    public Cliente(string tipoCliente, string nombre, string ci, int edad, string telefono)
    {
        TipoCliente = tipoCliente;
        Nombre = nombre;
        Ci = ci;
        Edad = edad;
        Telefono = telefono;
    }
}