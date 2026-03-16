namespace ProyectoArqSoft.Domain.Entities.Medicamento;

public class Antibiotico : Medicamento
{
    public bool RequiereReceta => true;
}
