using ProyectoArqSoft.Models;

namespace ProyectoArqSoft.FactoryProducts
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario? GetByEmail(string email);
        Usuario? GetByUserName(string userName);
        bool ExisteEmail(string email);
        bool ExisteUserName(string userName);
        int CambiarPassword(int idUsuario, string nuevoPasswordHash, bool mustChangePassword);

        int UpdateDatosEdicion(Usuario usuario);
    }
}