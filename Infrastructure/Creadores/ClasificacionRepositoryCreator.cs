//using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.FactoryCreators;
//using ProyectoArqSoft.Domain.Model.Clasificacion;
using ProyectoArqSoft.Models;
//using ProyectoArqSoft.Infrastructure.Repositories;
using ProyectoArqSoft.FactoryProducts;

//amespace ProyectoArqSoft.Infrastructure.Creadores
namespace ProyectoArqSoft.FactoryCreators
{
    public class ClasificacionRepositoryCreator : RepositoryCreator<Clasificacion>
    {
        public override IRepository<Clasificacion> CreateRepo()
        {
            return new ClasificacionRepository();
        }
    }
}