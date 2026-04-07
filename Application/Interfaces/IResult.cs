using ProyectoArqSoft.Domain.Validators;

namespace ProyectoArqSoft.Aplication.Interfaces
{
    public interface IResult<T>
    {
        Result Validar(T entidad);
    }
}