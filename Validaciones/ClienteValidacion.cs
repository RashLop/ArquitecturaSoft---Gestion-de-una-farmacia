using ProyectoArqSoft.Models;
using System.Text.RegularExpressions;

namespace ProyectoArqSoft.Validaciones
{
    public class ClienteValidacion : IValidacion<Cliente>
    {
        public List<string> Errores { get; private set; } = new List<string>();

        public bool EsValido(Cliente cliente)
        {
            Errores.Clear();

            ValidarTipoCliente(cliente.Tipo_Cliente);
            ValidarNombre(cliente.Nombre);
            ValidarApellidoPaterno(cliente.Apellido_Paterno);
            ValidarApellidoMaterno(cliente.Apellido_Materno);
            ValidarCi(cliente.Ci, cliente.Ci_Extencion);
            ValidarFechaNacimiento(cliente.Fecha_De_Nacimiento);
            ValidarTelefono(cliente.Telefono);

            return Errores.Count == 0;
        }

        private void ValidarTipoCliente(string tipoCliente)
        {
            if (string.IsNullOrWhiteSpace(tipoCliente))
            {
                Errores.Add("El tipo de cliente es requerido");
                return;
            }

            string[] tiposValidos = { "Regular", "Frecuente", "VIP" };
            if (!tiposValidos.Contains(tipoCliente))
            {
                Errores.Add("El tipo de cliente debe ser Regular, Frecuente o VIP");
            }
        }

        private void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Errores.Add("El nombre es requerido");
                return;
            }

            nombre = nombre.Trim();

            if (nombre.Length < 2)
            {
                Errores.Add("El nombre debe tener al menos 2 caracteres");
            }

            if (nombre.Length > 45)
            {
                Errores.Add("El nombre no puede tener más de 45 caracteres");
            }

            if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                Errores.Add("El nombre solo puede contener letras y espacios");
            }
        }

        private void ValidarApellidoPaterno(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
            {
                Errores.Add("El apellido paterno es requerido");
                return;
            }

            apellido = apellido.Trim();

            if (apellido.Length < 2)
            {
                Errores.Add("El apellido paterno debe tener al menos 2 caracteres");
            }

            if (apellido.Length > 45)
            {
                Errores.Add("El apellido paterno no puede tener más de 45 caracteres");
            }

            if (!Regex.IsMatch(apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                Errores.Add("El apellido paterno solo puede contener letras y espacios");
            }
        }

        private void ValidarApellidoMaterno(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
            {
                Errores.Add("El apellido materno es requerido");
                return;
            }

            apellido = apellido.Trim();

            if (apellido.Length < 2)
            {
                Errores.Add("El apellido materno debe tener al menos 2 caracteres");
            }

            if (apellido.Length > 45)
            {
                Errores.Add("El apellido materno no puede tener más de 45 caracteres");
            }

            if (!Regex.IsMatch(apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                Errores.Add("El apellido materno solo puede contener letras y espacios");
            }
        }

        private void ValidarCi(string ci, string ciExtencion)
        {
            if (string.IsNullOrWhiteSpace(ci))
            {
                Errores.Add("El CI es requerido");
                return;
            }

            ci = ci.Trim();

            if (!Regex.IsMatch(ci, @"^\d+$"))
            {
                Errores.Add("El CI debe contener solo números");
            }

            if (ci.Length < 5)
            {
                Errores.Add("El CI debe tener al menos 5 dígitos");
            }

            if (ci.Length > 15)
            {
                Errores.Add("El CI no puede tener más de 15 dígitos");
            }

            // Validar extensión del CI
            if (!string.IsNullOrWhiteSpace(ciExtencion))
            {
                string[] extensionesValidas = { "LP", "CB", "SC", "CH", "OR", "PT", "TJ", "BN", "PD" };

                if (!extensionesValidas.Contains(ciExtencion.ToUpper()))
                {
                    Errores.Add("La extensión del CI no es válida");
                }
            }
        }

        private void ValidarFechaNacimiento(DateTime fechaNacimiento)
        {
            if (fechaNacimiento == DateTime.MinValue)
            {
                Errores.Add("La fecha de nacimiento es requerida");
                return;
            }

            // Calcular edad
            var today = DateTime.Today;
            var edad = today.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > today.AddYears(-edad)) edad--;

            if (edad < 18)
            {
                Errores.Add("El cliente debe ser mayor de 18 años");
            }

            if (edad > 120)
            {
                Errores.Add("La edad no puede ser mayor a 120 años");
            }

            if (fechaNacimiento > DateTime.Today)
            {
                Errores.Add("La fecha de nacimiento no puede ser futura");
            }
        }

        private void ValidarTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
            {
                Errores.Add("El teléfono es requerido");
                return;
            }

            telefono = telefono.Trim();

            if (!Regex.IsMatch(telefono, @"^[0-9-+\s]+$"))
            {
                Errores.Add("El teléfono solo puede contener números, guiones y espacios");
            }

            if (telefono.Replace("-", "").Replace(" ", "").Replace("+", "").Length < 7)
            {
                Errores.Add("El teléfono debe tener al menos 7 dígitos");
            }

            if (telefono.Length > 15)
            {
                Errores.Add("El teléfono no puede tener más de 15 caracteres");
            }
        }

        public string ObtenerMensajesError()
        {
            return string.Join("<br/>", Errores);
        }
    }
}