using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public interface IClienteService
    {
        DataTable ObtenerTodos();
        DataTable ObtenerTodos(string filtro);
        Cliente? ObtenerPorId(int id);

        Validacion Crear(
            bool esConsumidorFinal,
            string nit,
            string razonSocial,
            string? correoElectronico);

        Validacion Actualizar(
            int id,
            bool esConsumidorFinal,
            string nit,
            string razonSocial,
            string? correoElectronico);

        Validacion Eliminar(int id);
    }
}
