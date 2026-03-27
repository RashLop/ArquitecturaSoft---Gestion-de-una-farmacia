using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Repositories;
using ProyectoArqSoft.Models;


namespace ProyectoArqSoft.FactoryCreators
{
    public class BioquimicoRepositoryCreator : RepositoryCreator<Bioquimico>
    {
        private readonly IConfiguration _configuration;

        public BioquimicoRepositoryCreator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override IRepository<Bioquimico> CreateRepo()
        {
            
            return new BioquimicoRepository(_configuration);
        }
    }
}