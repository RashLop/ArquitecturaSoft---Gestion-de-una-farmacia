using ProyectoArqSoft.FactoryCreators;
using ProyectoArqSoft;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.FactoryProducts;


namespace ProyectoArqSoft.FactoryCreators
{
    public class UsuarioRepositoryCreator : RepositoryCreator<Usuario>
    {
        public override IRepository<Usuario> CreateRepo()
        {
            
            return new UsuarioRepository();
        }
    }
}