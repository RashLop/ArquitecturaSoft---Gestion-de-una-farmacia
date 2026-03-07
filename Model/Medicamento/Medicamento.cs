using Microsoft.AspNetCore.SignalR;

namespace ProyectoArqSoft.Modelos ; 
public class Medicamento
{
    public  int id_medicamento {get; set; }
    public string nombre {get; set; } = string.Empty; 
    public string presentacion {get; set; } = string.Empty; 
    public string clasificacion {get; set; } = string.Empty;
    public string concentracion {get; set; } = string.Empty; 
   
    public int stock_minimo {get; set; }
}