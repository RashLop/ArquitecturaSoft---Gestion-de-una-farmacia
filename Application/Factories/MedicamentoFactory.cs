using ProyectoArqSoft.Domain.Entities.Medicamento;
using ProyectoArqSoft.Domain.Enums;

namespace ProyectoArqSoft.Application.Factories;

public class MedicamentoFactory : IMedicamentoFactory
{
    public Medicamento Crear(ClasificacionMedicamento clasificacion)
    {
        Console.WriteLine("Factory: Decidiendo medicamento a crear");
        return clasificacion switch
        {


            ClasificacionMedicamento.Antibiotico => new Antibiotico(),

            ClasificacionMedicamento.Analgesico => new Analgesico(),

            ClasificacionMedicamento.Antiinflamatorio => new Antiinflamatorio(),

            ClasificacionMedicamento.Antialergico => new Antialergico(),

            ClasificacionMedicamento.Antipiretico => new Antipiretico(),

            ClasificacionMedicamento.Vitamina => new Vitamina(),

            ClasificacionMedicamento.Antiseptico => new Antiseptico(),

            _ => new Medicamento()
        };
    }
}
