using ProyectoArqSoft.DTO;
using ProyectoArqSoft.FactoryProducts;

namespace ProyectoArqSoft.Validaciones
{
    public class UsuarioNegocioValidacion
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioNegocioValidacion(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public Result ValidarRegistro(UsuarioRegistroDto dto)
        {
            if (_repository.ExisteEmail(dto.Email))
                return Result.Fail("El correo electrónico ya está registrado.");

            return Result.Ok();
        }

        // 🔹 Validación para actualización
        public Result ValidarActualizacion(UsuarioActualizacionDto dto)
        {
            var usuarioActual = _repository.GetById(dto.IdUsuario);

            if (usuarioActual == null)
                return Result.Fail("El usuario no existe.");

            var usuarioConMismoEmail = _repository.GetByEmail(dto.Email);
            if (usuarioConMismoEmail != null && usuarioConMismoEmail.IdUsuario != dto.IdUsuario)
                return Result.Fail("El correo electrónico ya está registrado.");

            var usuarioConMismoUserName = _repository.GetByUserName(dto.UserName);
            if (usuarioConMismoUserName != null && usuarioConMismoUserName.IdUsuario != dto.IdUsuario)
                return Result.Fail("El nombre de usuario ya está en uso.");

            return Result.Ok();
        }

        public Result ValidarEliminacion(int idUsuario)
        {
            if (idUsuario <= 0)
                return Result.Fail("El identificador del usuario no es válido.");

            var usuario = _repository.GetById(idUsuario);
            if (usuario == null)
                return Result.Fail("El usuario no existe.");

            return Result.Ok();
        }
    }
}