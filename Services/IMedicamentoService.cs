using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public interface IMedicamentoService
    {
        DataTable ObtenerTodos();
        DataTable ObtenerTodos(string filtro);
        Medicamento? ObtenerPorId(int id);

        Validacion Crear(
            string nombre,
            string presentacion,
            string clasificacion,
            string concentracion,
            decimal precio,
            int stock);

        Validacion Actualizar(
            int id,
            string nombre,
            string presentacion,
            string clasificacion,
            string concentracion,
            decimal precio,
            int stock);

        Validacion EliminarLogicamente(int id);
    }
}