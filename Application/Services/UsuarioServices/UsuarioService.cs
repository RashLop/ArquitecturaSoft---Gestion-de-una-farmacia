using System.Data;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IValidacion<UsuarioRegistroDto> _registroValidador;
        private readonly IValidacion<UsuarioActualizacionDto> _actualizacionValidador;
        private readonly UsuarioNegocioValidacion _negocioValidador;

        public UsuarioService(
            IUsuarioRepository repository,
            IValidacion<UsuarioRegistroDto> registroValidador,
            IValidacion<UsuarioActualizacionDto> actualizacionValidador,
            UsuarioNegocioValidacion negocioValidador)
        {
            _repository = repository;
            _registroValidador = registroValidador;
            _actualizacionValidador = actualizacionValidador;
            _negocioValidador = negocioValidador;
        }

        public Validacion CrearUsuario(UsuarioRegistroDto dto, string role)
        {
            Validacion validacionEntrada = _registroValidador.Validar(dto);
            if (!validacionEntrada.IsSuccess)
                return validacionEntrada;

            role = role?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(role))
                return Validacion.Fail("El rol es obligatorio.");

            Validacion validacionNegocio = _negocioValidador.ValidarRegistro(dto);
            if (!validacionNegocio.IsSuccess)
                return validacionNegocio;

            string passwordHash = PasswordHelper.Hash(dto.Password);

            Usuario usuario = new Usuario
            {
                Email = dto.Email.Trim(),
                UserName = dto.UserName.Trim(),
                PasswordHash = passwordHash,
                Role = role,
                MustChangePassword = 0,
                IsActive = 1,
                BioquimicoIdBioquimico = null
            };

            int filasAfectadas = _repository.Insert(usuario);

            if (filasAfectadas <= 0)
                return Validacion.Fail("No se pudo registrar el usuario.");

            return Validacion.Ok();
        }

        public Validacion ActualizarUsuario(UsuarioActualizacionDto dto)
        {

            Validacion validacionEntrada = _actualizacionValidador.Validar(dto);
            if (!validacionEntrada.IsSuccess)
                return validacionEntrada;

            Validacion validacionNegocio = _negocioValidador.ValidarActualizacion(dto);
            if (!validacionNegocio.IsSuccess)
                return validacionNegocio;

            Usuario? usuarioActual = _repository.GetById(dto.IdUsuario);
            if (usuarioActual == null)
                return Validacion.Fail("El usuario no existe.");

            usuarioActual.Email = dto.Email.Trim();
            usuarioActual.UserName = dto.UserName.Trim();
            usuarioActual.Role = dto.Role.Trim();

            int filasAfectadas = _repository.Update(usuarioActual);

            if (filasAfectadas <= 0)
                return Validacion.Fail("No se pudo actualizar el usuario.");

            return Validacion.Ok();
        }

        public Validacion EliminarUsuario(int idUsuario)
        {
            Validacion validacionNegocio = _negocioValidador.ValidarEliminacion(idUsuario);
            if (!validacionNegocio.IsSuccess)
                return validacionNegocio;

            Usuario? usuario = _repository.GetById(idUsuario);
            if (usuario == null)
                return Validacion.Fail("El usuario no existe.");

            int filasAfectadas = _repository.Delete(usuario);

            if (filasAfectadas <= 0)
                return Validacion.Fail("No se pudo eliminar el usuario.");

            return Validacion.Ok();
        }

        public UsuarioDto? ObtenerUsuarioPorId(int idUsuario)
        {
            if (idUsuario <= 0)
                return null;

            Usuario? usuario = _repository.GetById(idUsuario);
            if (usuario == null)
                return null;

            return MapearDto(usuario);
        }

        public UsuarioDto? ObtenerUsuarioPorEmail(string email)
        {
            email = email?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
                return null;

            Usuario? usuario = _repository.GetByEmail(email);
            if (usuario == null)
                return null;

            return MapearDto(usuario);
        }

        public UsuarioDto? ObtenerUsuarioPorUserName(string userName)
        {
            userName = userName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            Usuario? usuario = _repository.GetByUserName(userName);
            if (usuario == null)
                return null;

            return MapearDto(usuario);
        }

        public DataTable ObtenerTodos()
        {
            return _repository.GetAll();
        }

        public DataTable ObtenerTodos(string filtro)
        {
            filtro = filtro?.Trim() ?? string.Empty;
            return _repository.GetAll(filtro);
        }

        public bool ExisteEmail(string email)
        {
            email = email?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return _repository.ExisteEmail(email);
        }

        public bool ExisteUserName(string userName)
        {
            userName = userName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(userName))
                return false;

            return _repository.ExisteUserName(userName);
        }

        private UsuarioDto MapearDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Role = usuario.Role
            };
        }
    }
}