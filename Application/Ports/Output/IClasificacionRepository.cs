//using ProyectoArqSoft.Domain.Model.Clasificacion;
using ProyectoArqSoft.Models;

//namespace ProyectoArqSoft.Application.Ports.Output
namespace ProyectoArqSoft.FactoryProducts
{
    public interface IClasificacionRepository : IRepository<Clasificacion>
    {
        bool TieneMedicamentosActivosAsociados(int idClasificacion);
        bool ExisteNombreActivo(string nombre);
        bool ExisteNombreActivoExcluyendoId(int idClasificacion, string nombre);
    }
}