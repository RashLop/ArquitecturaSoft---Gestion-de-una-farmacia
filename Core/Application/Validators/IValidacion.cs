namespace ProyectoArqSoft.Validaciones
{
    public interface IValidacion<T>
    {
        Validacion Validar(T entidad);
    }
}