using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public interface IBioquimicoService
    {
        DataTable ObtenerTodos(string filtro);
        Bioquimico? ObtenerPorId(int id);
        Validacion Crear(Bioquimico bioquimico);
        Validacion Actualizar(Bioquimico bioquimico);
        Validacion Eliminar(int id);
    }
}