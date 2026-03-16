using ProyectoArqSoft.Domain.Entities.Medicamento;

namespace ProyectoArqSoft.Domain.Interfaces
{
    public interface IMedicamentoRepository
    {
        void Crear(Medicamento medicamento);
        void Actualizar(Medicamento medicamento);
        Medicamento ObtenerPorId(int id);
        void EliminarLogico(int id);
    }
}
