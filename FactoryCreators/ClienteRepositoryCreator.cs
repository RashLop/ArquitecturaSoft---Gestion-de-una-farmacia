using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.FactoryCreators
{
    public class ClienteRepositoryCreator : RepositoryCreator<Cliente>
    {
        private readonly IConfiguration configuration;

        public ClienteRepositoryCreator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override IRepository<Cliente> CreateRepo()
        {
            return new ClienteRepository(configuration);
        }
    }
}
