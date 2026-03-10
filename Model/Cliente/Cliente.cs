using System.ComponentModel.DataAnnotations;

namespace ProyectoArqSoft.Models;

public class Cliente
{
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El tipo de cliente es requerido")]
    public string TipoCliente { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El CI es requerido")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "El CI debe tener entre 5 y 20 dígitos")]
    public string Ci { get; set; }

    [Required(ErrorMessage = "La edad es requerida")]
    [Range(18, 120, ErrorMessage = "La edad debe estar entre 18 y 120 años")]
    public int Edad { get; set; }

    [Required(ErrorMessage = "El teléfono es requerido")]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 20 caracteres")]
    public string Telefono { get; set; }

    public Cliente() { }

    public Cliente(string tipoCliente, string nombre, string ci, int edad, string telefono)
    {
        TipoCliente = tipoCliente;
        Nombre = nombre;
        Ci = ci;
        Edad = edad;
        Telefono = telefono;
    }
}