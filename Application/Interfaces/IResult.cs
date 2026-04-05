namespace ProyectoArqSoft.Validaciones
{
    public interface IResult<T>
    {
        Result Validar(T entidad);
    }
}