using System.Data;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public interface IUsuarioService
    {
        Result CrearUsuario(UsuarioRegistroDto dto, string role);
        Result ActualizarUsuario(UsuarioActualizacionDto dto, int? idUsuarioSesion);
        Result EliminarUsuario(int idUsuario, int? idUsuarioSesion);

        UsuarioDto? ObtenerUsuarioPorId(int idUsuario);
        UsuarioDto? ObtenerUsuarioPorEmail(string email);
        UsuarioDto? ObtenerUsuarioPorUserName(string userName);

        DataTable ObtenerTodos();
        DataTable ObtenerTodos(string filtro);

        bool ExisteEmail(string email);
        bool ExisteUserName(string userName);

        Result ValidarActivacionCuenta(string token);
        Result ActivarCuenta(string token, string nuevaPassword);

        Result ActualizarUsuarioEdicion(UsuarioEdicionDto dto, int? idUsuarioSesion);
    }
}