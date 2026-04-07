using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Infrastructure.Persistence.Repositories;

namespace ProyectoArqSoft.Infrastructure.Creadores
{
    public class UsuarioTokenRepositoryCreator
    {
        public IUsuarioTokenRepository CreateRepo()
        {
            return new UsuarioTokenRepository();
        }
    }
}