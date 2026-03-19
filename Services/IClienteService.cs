using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public interface IClienteService
    {
        DataTable ObtenerTodos(string? filtro = null);
        Cliente? ObtenerPorId(int id);
        Validacion Crear(string nit, string razonSocial, string? correoElectronico, DateTime fechaRegistro);
        Validacion Actualizar(int id, string nit, string razonSocial, string? correoElectronico, DateTime fechaRegistro);
        Validacion EliminarLogicamente(int id);
    }
}