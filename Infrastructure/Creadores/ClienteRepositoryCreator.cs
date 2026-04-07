using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Infrastructure.Persistence.Repositories;

namespace ProyectoArqSoft.Infrastructure.Creadores
{
    public class ClienteRepositoryCreator : RepositoryCreator<Cliente>
    {
        public override IRepository<Cliente> CreateRepo()
        {
            return new ClienteRepository();
        }
    }
}
