using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El CI es requerido")]
        [Display(Name = "Carnet de Identidad")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El CI debe tener entre 5 y 20 dígitos")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El CI solo puede contener números")]
        public string Ci { get; set; }

        [Required(ErrorMessage = "La edad es requerida")]
        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120 años")]
        [ValidarEdadMinima(18, ErrorMessage = "El cliente debe ser mayor de 18 años para registrarse")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 20 dígitos")]
        [RegularExpression(@"^[\d\s\+\-\(\)]+$", ErrorMessage = "El teléfono solo puede contener números, espacios, +, -, (),")]
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

        // Método de validación de negocio
        public bool EsMayorDeEdad()
        {
            return Edad >= 18;
        }

        // Método para formatear teléfono
        public string FormatearTelefono()
        {
            if (string.IsNullOrEmpty(Telefono))
                return "";

            // Eliminar caracteres no numéricos
            string soloNumeros = Regex.Replace(Telefono, @"[^\d]", "");

            if (soloNumeros.Length == 8)
                return Regex.Replace(soloNumeros, @"(\d{4})(\d{4})", "$1-$2");
            else if (soloNumeros.Length == 7)
                return Regex.Replace(soloNumeros, @"(\d{3})(\d{4})", "$1-$2");

            return Telefono;
        }
    }

    // Validador personalizado para edad mínima
    public class ValidarEdadMinima : ValidationAttribute
    {
        private readonly int _edadMinima;

        public ValidarEdadMinima(int edadMinima)
        {
            _edadMinima = edadMinima;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int edad = (int)value;
                if (edad < _edadMinima)
                {
                    return new ValidationResult(ErrorMessage ?? $"El cliente debe ser mayor de {_edadMinima} años");
                }
            }
            return ValidationResult.Success;
        }
    }
}