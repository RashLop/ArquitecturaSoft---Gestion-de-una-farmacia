using Microsoft.AspNetCore.SignalR;

namespace ProyectoArqSoft.Models ; 
public class Medicamento
{
    public  int Id {get; set; }
    public string Nombre {get; set; } 
    public string Presentacion {get; set; } 
    public string Clasificacion {get; set; } 
    public string Concentracion {get; set; } 
    public decimal Precio {get; set; }
    public int Stock { get; set; }

    //public short Estado { get; set; }
    //public DateTime FechaRegistro { get; set; }
    //public DateTime UltimaActualizacion { get; set; }
    
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