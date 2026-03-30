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

        public Validacion ValidarRegistro(UsuarioRegistroDto dto)
        {
            if (_repository.ExisteEmail(dto.Email))
                return Validacion.Fail("El correo electrónico ya está registrado.");

            if (_repository.ExisteUserName(dto.UserName))
                return Validacion.Fail("El nombre de usuario ya está en uso.");

            return Validacion.Ok();
        }

        // 🔹 Validación para actualización
        public Validacion ValidarActualizacion(UsuarioActualizacionDto dto)
        {
            var usuarioActual = _repository.GetById(dto.IdUsuario);

            if (usuarioActual == null)
                return Validacion.Fail("El usuario no existe.");

            var usuarioConMismoEmail = _repository.GetByEmail(dto.Email);
            if (usuarioConMismoEmail != null && usuarioConMismoEmail.IdUsuario != dto.IdUsuario)
                return Validacion.Fail("El correo electrónico ya está registrado.");

            var usuarioConMismoUserName = _repository.GetByUserName(dto.UserName);
            if (usuarioConMismoUserName != null && usuarioConMismoUserName.IdUsuario != dto.IdUsuario)
                return Validacion.Fail("El nombre de usuario ya está en uso.");

            return Validacion.Ok();
        }

        public Validacion ValidarEliminacion(int idUsuario)
        {
            if (idUsuario <= 0)
                return Validacion.Fail("El identificador del usuario no es válido.");

            var usuario = _repository.GetById(idUsuario);
            if (usuario == null)
                return Validacion.Fail("El usuario no existe.");

            return Validacion.Ok();
        }
    }
}