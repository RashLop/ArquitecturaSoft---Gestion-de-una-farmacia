using ProyectoArqSoft.FactoryProducts;

namespace ProyectoArqSoft.FactoryCreators
{
    public abstract class RepositoryCreator<T> //Clase creadora
    {
        public abstract IRepository<T> CreateRepo();

    }
}
