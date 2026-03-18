using ProyectoArqSoft.Models;
using ProyectoArqSoft.FactoryProducts;

namespace ProyectoArqSoft.FactoryCreators
{
    public class MedicamentoRepositoryCreator : RepositoryCreator<Medicamento>
    {
        private readonly IConfiguration configuration;

        public MedicamentoRepositoryCreator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override IRepository<Medicamento> CreateRepo()
        {
            return new MedicamentoRepository(configuration);
        }
    }
}