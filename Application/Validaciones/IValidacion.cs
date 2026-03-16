namespace ProyectoArqSoft.Application.Validaciones
{
    public interface IValidacion<T>
    {
        Validacion Validar(T entidad);
    }
}