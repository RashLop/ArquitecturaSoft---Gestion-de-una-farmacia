<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;

namespace ProyectoArqSoft.Model
{
    public class Cliente
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El tipo de cliente es requerido")]
        [Display(Name = "Tipo de Cliente")]
        public string TipoCliente { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 45 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El CI es requerido")]
        [Display(Name = "Carnet de Identidad")]
        [StringLength(45, MinimumLength = 5, ErrorMessage = "El CI debe tener entre 5 y 45 caracteres")]
        public string Ci { get; set; }

        [Required(ErrorMessage = "La edad es requerida")]
        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        [StringLength(45, ErrorMessage = "El teléfono no puede tener más de 45 caracteres")]
        public string Telefono { get; set; }

        // Constructores
        public Cliente() { }

        public Cliente(string tipoCliente, string nombre, string ci, int edad, string telefono)
        {
            TipoCliente = tipoCliente;
            Nombre = nombre;
            Ci = ci;
            Edad = edad;
            Telefono = telefono;
        }
=======
namespace ProyectoArqSoft.Model.Cliente;

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
>>>>>>> 23851e3e2c1bddc3014d5a342e12d51d3f5c7db5
    }
}