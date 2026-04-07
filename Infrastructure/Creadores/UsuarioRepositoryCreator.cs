using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Infrastructure.Persistence.Repositories;


namespace ProyectoArqSoft.Infrastructure.Creadores
{
    public class UsuarioRepositoryCreator
    {
        public IUsuarioRepository CreateRepo()
        {
            return new UsuarioRepository();
        }
    }
}