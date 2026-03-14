using ProyectoArqSoft.Models;
using System.Text.RegularExpressions;

namespace ProyectoArqSoft.Validaciones
{
    public class MedicamentoValidacion : IValidacion<Medicamento>
    {
        public List<string> Errores { get; private set; } = new List<string>();

        public bool EsValido(Medicamento medicamento)
        {
            Errores.Clear();

            ValidarNombre(medicamento.Nombre);
            ValidarPresentacion(medicamento.Presentacion);
            ValidarClasificacion(medicamento.Clasificacion);
            ValidarConcentracion(medicamento.Concentracion);
            ValidarPrecio(medicamento.Precio);
            ValidarStock(medicamento.Stock);

            return Errores.Count == 0;
        }

        private void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Errores.Add("El nombre del medicamento es requerido");
                return;
            }

            nombre = nombre.Trim();

            if (nombre.Length < 3)
            {
                Errores.Add("El nombre debe tener al menos 3 caracteres");
            }

            if (nombre.Length > 100)
            {
                Errores.Add("El nombre no puede tener más de 100 caracteres");
            }

            if (!Regex.IsMatch(nombre, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-]+$"))
            {
                Errores.Add("El nombre solo puede contener letras, números, espacios y guiones");
            }
        }

        private void ValidarPresentacion(string presentacion)
        {
            if (string.IsNullOrWhiteSpace(presentacion))
            {
                Errores.Add("La presentación es requerida");
                return;
            }

            string[] presentacionesValidas = {
                "Tabletas", "Cápsulas", "Jarabe", "Inyectable",
                "Crema", "Gotas", "Suspensión", "Polvo",
                "Ampollas", "Supositorios", "Parche", "Inhalador",
                "Solución", "Emulsión", "Gel", "Ungüento"
            };

            if (!presentacionesValidas.Contains(presentacion))
            {
                Errores.Add("La presentación seleccionada no es válida");
            }
        }

        private void ValidarClasificacion(string clasificacion)
        {
            if (string.IsNullOrWhiteSpace(clasificacion))
            {
                Errores.Add("La clasificación es requerida");
                return;
            }

            string[] clasificacionesValidas = {
                "Analgésico", "Antiinflamatorio", "Antibiótico",
                "Antihistamínico", "Antidepresivo", "Antihipertensivo",
                "Antidiabético", "Antifúngico", "Antiviral",
                "Vacuna", "Suplemento", "Ansiolítico",
                "Anticonvulsivo", "Antipsicótico", "Relajante Muscular",
                "Antiácido", "Laxante", "Antidiarreico", "Otros"
            };

            if (!clasificacionesValidas.Contains(clasificacion))
            {
                Errores.Add("La clasificación seleccionada no es válida");
            }
        }

        private void ValidarConcentracion(string concentracion)
        {
            if (string.IsNullOrWhiteSpace(concentracion))
            {
                Errores.Add("La concentración es requerida");
                return;
            }

            concentracion = concentracion.Trim();

            if (concentracion.Length < 2)
            {
                Errores.Add("La concentración debe tener al menos 2 caracteres");
            }

            if (concentracion.Length > 50)
            {
                Errores.Add("La concentración no puede tener más de 50 caracteres");
            }

            // Ejemplos válidos: 500mg, 1g, 10mg/ml, 5%, 100mg/5ml
            if (!Regex.IsMatch(concentracion, @"^[\d\s\.\,]+(mg|g|mcg|UI|ml|%|mg\/ml|g\/ml|mcg\/ml|mg\/5ml)?$", RegexOptions.IgnoreCase))
            {
                Errores.Add("La concentración debe tener un formato válido (ej: 500mg, 1g, 10mg/ml, 5%)");
            }
        }

        private void ValidarPrecio(decimal precio)
        {
            if (precio <= 0)
            {
                Errores.Add("El precio debe ser mayor a 0");
                return;
            }

            if (precio > 10000)
            {
                Errores.Add("El precio no puede ser mayor a Bs. 10,000");
            }

            // Validar que no tenga más de 2 decimales
            if (decimal.Round(precio, 2) != precio)
            {
                Errores.Add("El precio no puede tener más de 2 decimales");
            }
        }

        private void ValidarStock(int stock)
        {
            if (stock < 0)
            {
                Errores.Add("El stock no puede ser negativo");
            }

            if (stock > 9999)
            {
                Errores.Add("El stock no puede ser mayor a 9999 unidades");
            }
        }

        public string ObtenerMensajesError()
        {
            return string.Join("<br/>", Errores);
        }
    }
}