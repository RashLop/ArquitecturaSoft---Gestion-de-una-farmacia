using ProyectoArqSoft.Models;
using ProyectoArqSoft.FactoryProducts;

namespace ProyectoArqSoft.FactoryCreators
{
    public class ClienteRepositoryCreator : RepositoryCreator<Cliente>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClienteRepository> _logger;

        public ClienteRepositoryCreator(IConfiguration configuration, ILogger<ClienteRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public override IRepository<Cliente> CreateRepo()
        {
            return new ClienteRepository(_configuration, _logger);
        }
    }
}