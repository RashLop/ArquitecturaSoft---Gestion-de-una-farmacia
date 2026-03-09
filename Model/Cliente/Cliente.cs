using System.ComponentModel.DataAnnotations;

namespace ProyectoArqSoft.Model.Cliente
{
    public class Cliente
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El tipo de cliente es requerido")]
        [Display(Name = "Tipo de Cliente")]
        public string TipoCliente { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El CI es requerido")]
        [Display(Name = "Carnet de Identidad")]
        public string Ci { get; set; }

        [Required(ErrorMessage = "La edad es requerida")]
        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string Telefono { get; set; }

        // Constructor vacío
        public Cliente()
        {
        }

        // Constructor con parámetros
        public Cliente(string tipoCliente, string nombre, string ci, int edad, string telefono)
        {
            TipoCliente = tipoCliente;
            Nombre = nombre;
            Ci = ci;
            Edad = edad;
            Telefono = telefono;
        }
    }
}