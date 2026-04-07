using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public interface IMedicamentoService
    {
        DataTable ObtenerTodos();
        DataTable ObtenerTodos(string filtro);
        Medicamento? ObtenerPorId(int id);

        Result Crear(
            string nombre,
            string presentacion,
            int idClasificacion,
            string concentracion,
            decimal precio,
            int stock);

        Result Actualizar(
            int id,
            string nombre,
            string presentacion,
            int idClasificacion,
            string concentracion,
            decimal precio,
            int stock);

        Result EliminarLogicamente(int id);
    }
}