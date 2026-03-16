using ProyectoArqSoft.Domain.Entities.Medicamento;
using ProyectoArqSoft.Domain.Enums;

namespace ProyectoArqSoft.Application.Factories;

public interface IMedicamentoFactory
{
    Medicamento Crear(ClasificacionMedicamento clasificacion);
}