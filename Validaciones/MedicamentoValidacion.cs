using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.Validaciones
{
    public class MedicamentoValidacion : IValidacion<Medicamento>
    {
        public Validacion Validar(Medicamento medicamento)
        {
            if (string.IsNullOrWhiteSpace(medicamento.Nombre))
                return new Validacion(false, "El nombre es obligatorio");

            if (medicamento.Precio <= 0)
                return new Validacion(false, "El precio debe ser mayor a 0");

            if (medicamento.Stock < 0)
                return new Validacion(false, "El stock no puede ser negativo");

            return new Validacion(true);
        }
    }
}