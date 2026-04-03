using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Repositories;

namespace ProyectoArqSoft.FactoryCreators
{
    public class UsuarioTokenRepositoryCreator
    {
        public IUsuarioTokenRepository CreateRepo()
        {
            return new UsuarioTokenRepository();
        }
    }
}