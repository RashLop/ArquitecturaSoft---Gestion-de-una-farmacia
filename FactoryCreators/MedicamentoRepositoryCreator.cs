using ProyectoArqSoft.Models;
using ProyectoArqSoft.FactoryProducts;

namespace ProyectoArqSoft.FactoryCreators
{
    public class MedicamentoRepositoryCreator : RepositoryCreator<Medicamento>
    {
        public override IRepository<Medicamento> CreateRepo()
        {
            throw new NotImplementedException();
        }
    }
}
