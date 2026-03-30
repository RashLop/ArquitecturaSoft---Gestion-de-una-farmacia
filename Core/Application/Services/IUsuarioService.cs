using System.Data;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public interface IUsuarioService
    {
        Validacion CrearUsuario(UsuarioRegistroDto dto, string role);
        Validacion ActualizarUsuario(int idUsuario, string email, string userName, string role);
        Validacion EliminarUsuario(int idUsuario);

        UsuarioDto? ObtenerUsuarioPorId(int idUsuario);
        UsuarioDto? ObtenerUsuarioPorEmail(string email);
        UsuarioDto? ObtenerUsuarioPorUserName(string userName);

        DataTable ObtenerTodos();
        DataTable ObtenerTodos(string filtro);

        bool ExisteEmail(string email);
        bool ExisteUserName(string userName);
    }
}