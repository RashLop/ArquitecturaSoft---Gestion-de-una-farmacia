namespace ProyectoArqSoft.Application.Validaciones
{
    public class Validacion
    {
        public bool EsValido { get; set; }
        public string MensajeError { get; set; }

        public Validacion(bool esValido, string mensajeError = "")
        {
            EsValido = esValido;
            MensajeError = mensajeError;
        }
    }
}