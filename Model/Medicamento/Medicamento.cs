using Microsoft.AspNetCore.SignalR;

namespace ProyectoArqSoft.Models ; 
public class Medicamento
{
    public  int Id {get; set; }
    public string Nombre {get; set; } = string.Empty; 
    public string Presentacion {get; set; } =string.Empty; 
    public string Clasificacion {get; set; } = string.Empty; 
    public string Concentracion {get; set; }  = string.Empty; 
    public decimal Precio {get; set; }  
    public int Stock { get; set; }

    public short Estado { get; set; } = 1; 
    public DateTime FechaRegistro { get; set; }
    public DateTime UltimaActualizacion { get; set; }
    
    public Medicamento()
    {
        
    }

    public Medicamento(string nombre, string presentacion, string clasificacion, string concentracion, decimal precio, int stock)
    {
        Nombre = nombre;
        Presentacion = presentacion;
        Clasificacion = clasificacion;
        Concentracion = concentracion;
        Precio = precio;
        Stock = stock;
    }
}