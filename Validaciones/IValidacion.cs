namespace ProyectoArqSoft.Validaciones
{
    public interface IValidacion<T>
    {
        List<string> Errores { get; }
        bool EsValido(T entidad);
        string ObtenerMensajesError();
    }
}