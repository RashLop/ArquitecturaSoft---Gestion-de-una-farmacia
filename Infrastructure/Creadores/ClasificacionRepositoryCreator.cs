using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Infrastructure.Persistence.Repositories;

namespace ProyectoArqSoft.Infrastructure.Creadores
{
    public class ClasificacionRepositoryCreator : RepositoryCreator<Clasificacion>
    {
        public override IRepository<Clasificacion> CreateRepo()
        {
            return new ClasificacionRepository();
        }
    }
}