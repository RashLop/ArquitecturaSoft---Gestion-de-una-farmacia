//using ProyectoArqSoft.Domain.Model.Clasificacion;
using ProyectoArqSoft.Models;
//using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Validaciones;
using System.Data;

//namespace ProyectoArqSoft.Application.Interfaces
namespace ProyectoArqSoft.Services

{
    public interface IClasificacionService
    {
        DataTable ObtenerTodos();
        DataTable ObtenerTodos(string filtro);
        Clasificacion? ObtenerPorId(int id);

        Result Crear(string nombre, string origen);
        Result Actualizar(int id, string nombre, string origen, int idUsuario);
        Result EliminarLogicamente(int id, int idUsuario);
    }
}