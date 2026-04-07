using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Repositories;


namespace ProyectoArqSoft.FactoryCreators
{
    public class UsuarioRepositoryCreator
    {
        public IUsuarioRepository CreateRepo()
        {
            return new UsuarioRepository();
        }
    }
}