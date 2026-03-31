using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.FactoryCreators
{
    public class ClienteRepositoryCreator : RepositoryCreator<Cliente>
    {
        public override IRepository<Cliente> CreateRepo()
        {
            return new ClienteRepository();
        }
    }
}
