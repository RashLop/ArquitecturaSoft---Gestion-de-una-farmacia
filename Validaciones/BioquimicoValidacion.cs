using ProyectoArqSoft.Models;
using System.Text.RegularExpressions;

namespace ProyectoArqSoft.Validaciones
{
    public class BioquimicoValidacion : IValidacion<Bioquimico>
    {
        public List<string> Errores { get; private set; } = new List<string>();

        public bool EsValido(Bioquimico bioquimico)
        {
            Errores.Clear();

            ValidarNombres(bioquimico.Nombres);
            ValidarApellidoPaterno(bioquimico.Apellido_Paterno);
            ValidarApellidoMaterno(bioquimico.Apellido_Materno);
            ValidarCi(bioquimico.Ci, bioquimico.Ci_Extencion);
            ValidarTelefono(bioquimico.Telefono);

            return Errores.Count == 0;
        }

        private void ValidarNombres(string nombres)
        {
            if (string.IsNullOrWhiteSpace(nombres))
            {
                Errores.Add("Los nombres son requeridos");
                return;
            }

            nombres = nombres.Trim();

            if (nombres.Length < 3)
            {
                Errores.Add("Los nombres deben tener al menos 3 caracteres");
            }

            if (nombres.Length > 45)
            {
                Errores.Add("Los nombres no pueden tener más de 45 caracteres");
            }

            if (!Regex.IsMatch(nombres, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                Errores.Add("Los nombres solo pueden contener letras y espacios");
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

            if (!string.IsNullOrWhiteSpace(ciExtencion))
            {
                string[] extensionesValidas = { "LP", "CB", "SC", "CH", "OR", "PT", "TJ", "BN", "PD" };

                if (!extensionesValidas.Contains(ciExtencion.ToUpper()))
                {
                    Errores.Add("La extensión del CI no es válida");
                }
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