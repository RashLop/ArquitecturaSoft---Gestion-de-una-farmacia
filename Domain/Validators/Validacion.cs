namespace ProyectoArqSoft.Validaciones
{
    public class Validacion
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        public Validacion(bool isSuccess, string error = "")
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Validacion Ok()
            => new Validacion(true);

        public static Validacion Fail(string error)
            => new Validacion(false, error);
    }
}