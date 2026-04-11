using System.Data;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Infrastructure.Helpers;
using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Application.Interfaces;

namespace ProyectoArqSoft.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IResult<UsuarioRegistroDto> _registroValidador;
        private readonly IResult<UsuarioActualizacionDto> _actualizacionValidador;
        private readonly UsuarioNegocioValidacion _negocioValidador;
        private readonly IUsuarioTokenService _usuarioTokenService;
        private readonly IEmailService _emailService;

        public UsuarioService(
            IUsuarioRepository repository,
            IResult<UsuarioRegistroDto> registroValidador,
            IResult<UsuarioActualizacionDto> actualizacionValidador,
            UsuarioNegocioValidacion negocioValidador,
            IUsuarioTokenService usuarioTokenService,
            IEmailService emailService)
        {
            _repository = repository;
            _registroValidador = registroValidador;
            _actualizacionValidador = actualizacionValidador;
            _negocioValidador = negocioValidador;
            _usuarioTokenService = usuarioTokenService;
            _emailService = emailService;
        }

        public Result CrearUsuario(UsuarioRegistroDto dto, string role, int? idUsuarioSesion)
        {
            Result validacion = ValidarCreacion(dto);
            if (!validacion.IsSuccess)
                return validacion;

            string passwordTemporal = Limpiar(dto.Password);
            string passwordHash = PasswordHelper.Hash(passwordTemporal);

            Usuario usuario = ConstruirUsuarioNuevo(dto, role, passwordHash, idUsuarioSesion);

            int filasAfectadas = _repository.Insert(usuario);
            if (filasAfectadas <= 0)
                return Result.Fail("No se pudo registrar el usuario.");

            Usuario? usuarioRegistrado = _repository.GetByEmail(usuario.Email);
            if (usuarioRegistrado == null)
                return Result.Fail("El usuario fue registrado, pero no se pudo recuperar su información.");

            return GenerarYEnviarActivacion(
                usuarioRegistrado.IdUsuario,
                usuarioRegistrado.Email,
                usuarioRegistrado.Nombres,
                usuarioRegistrado.UserName,
                passwordTemporal
            );
        }

        public Result ActualizarUsuario(UsuarioActualizacionDto dto, int? idUsuarioSesion)
        {
            Result validacion = ValidarActualizacion(dto);
            if (!validacion.IsSuccess)
                return validacion;

            Usuario? usuarioActual = _repository.GetById(dto.IdUsuario);
            if (usuarioActual == null)
                return Result.Fail("El usuario no existe.");

            AplicarActualizacion(usuarioActual, dto);

            int filasAfectadas = _repository.Update(usuarioActual, idUsuarioSesion);
            return filasAfectadas > 0
                ? Result.Ok()
                : Result.Fail("No se pudo actualizar el usuario.");
        }

        public Result EliminarUsuario(int idUsuario, int? idUsuarioSesion)
        {
            Result validacionNegocio = _negocioValidador.ValidarEliminacion(idUsuario);
            if (!validacionNegocio.IsSuccess)
                return validacionNegocio;

            Usuario? usuario = _repository.GetById(idUsuario);
            if (usuario == null)
                return Result.Fail("El usuario no existe.");

            int filasAfectadas = _repository.SoftDelete(usuario, idUsuarioSesion);
            return filasAfectadas > 0
                ? Result.Ok()
                : Result.Fail("No se pudo eliminar el usuario.");
        }

        public UsuarioDto? ObtenerUsuarioPorId(int idUsuario)
        {
            if (idUsuario <= 0)
                return null;

            return ObtenerYMapear(() => _repository.GetById(idUsuario));
        }

        public UsuarioDto? ObtenerUsuarioPorEmail(string email)
        {
            email = Limpiar(email);
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return ObtenerYMapear(() => _repository.GetByEmail(email));
        }

        public UsuarioDto? ObtenerUsuarioPorUserName(string userName)
        {
            userName = Limpiar(userName);
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            return ObtenerYMapear(() => _repository.GetByUserName(userName));
        }

        public DataTable ObtenerTodos()
        {
            return _repository.GetAll();
        }

        public DataTable ObtenerTodos(string filtro)
        {
            return _repository.GetAll(Limpiar(filtro));
        }

        public bool ExisteEmail(string email)
        {
            email = Limpiar(email);
            return !string.IsNullOrWhiteSpace(email) && _repository.ExisteEmail(email);
        }

        public bool ExisteUserName(string userName)
        {
            userName = Limpiar(userName);
            return !string.IsNullOrWhiteSpace(userName) && _repository.ExisteUserName(userName);
        }

        public Result ValidarActivacionCuenta(string token)
        {
            UsuarioToken? tokenValido = ObtenerTokenActivacionValido(token);
            return tokenValido != null
                ? Result.Ok()
                : Result.Fail("El token no es válido, ya fue usado o expiró.");
        }

        public Result ActivarCuenta(string token, string nuevaPassword)
        {
            nuevaPassword = Limpiar(nuevaPassword);
            if (string.IsNullOrWhiteSpace(nuevaPassword))
                return Result.Fail("La nueva contraseña es obligatoria.");

            UsuarioToken? tokenValido = ObtenerTokenActivacionValido(token);
            if (tokenValido == null)
                return Result.Fail("El token no es válido, ya fue usado o expiró.");

            string nuevoPasswordHash = PasswordHelper.Hash(nuevaPassword);

            int filasPassword = _repository.CambiarPassword(
                tokenValido.UsuarioIdUsuario,
                nuevoPasswordHash,
                false
            );

            if (filasPassword <= 0)
                return Result.Fail("No se pudo actualizar la contraseña del usuario.");

            Result resultadoToken = _usuarioTokenService.MarcarComoUsado(tokenValido.IdUsuarioToken);
            if (!resultadoToken.IsSuccess)
                return resultadoToken;

            return Result.Ok();
        }

        private Result ValidarCreacion(UsuarioRegistroDto dto)
        {
            Result validacionEntrada = _registroValidador.Validar(dto);
            if (!validacionEntrada.IsSuccess)
                return validacionEntrada;

            return _negocioValidador.ValidarRegistro(dto);
        }

        private Result ValidarActualizacion(UsuarioActualizacionDto dto)
        {
            Result validacionEntrada = _actualizacionValidador.Validar(dto);
            if (!validacionEntrada.IsSuccess)
                return validacionEntrada;

            return _negocioValidador.ValidarActualizacion(dto);
        }

        private Result GenerarYEnviarActivacion(
            int idUsuario,
            string email,
            string nombres,
            string userName,
            string passwordTemporal)
        {
            UsuarioTokenGeneracionDto tokenDto = new UsuarioTokenGeneracionDto
            {
                IdUsuario = idUsuario,
                TipoToken = TipoTokenConstantes.ActivacionCuenta,
                MinutosExpiracion = 60
            };

            Result validacionToken = _usuarioTokenService.GenerarToken(tokenDto, out string tokenPlano);
            if (!validacionToken.IsSuccess)
                return validacionToken;

            string tokenSeguro = Uri.EscapeDataString(tokenPlano);
            string enlaceActivacion = $"http://localhost:5081/Auth/ActivarCuenta?token={tokenSeguro}";

            return _emailService.EnviarCorreoActivacionCuenta(
                email,
                nombres,
                userName,
                passwordTemporal,
                enlaceActivacion
            );
        }

        private Usuario ConstruirUsuarioNuevo(UsuarioRegistroDto dto, string role, string passwordHash, int? idUsuarioSesion)
        {
            return new Usuario
            {
                Nombres = Limpiar(dto.Nombres),
                ApellidoPaterno = Limpiar(dto.ApellidoPaterno),
                ApellidoMaterno = dto.ApellidoMaterno?.Trim(),
                Ci = Limpiar(dto.Ci),
                CiExtencion = LimpiarMayus(dto.CiExtencion),
                Telefono = Limpiar(dto.Telefono),
                Email = Limpiar(dto.Email),
                UserName = Limpiar(dto.UserName),
                PasswordHash = passwordHash,
                Role = Limpiar(role),
                Activo = 1,
                MustChangePassword = 1,
                IdUsuarioCreador = idUsuarioSesion
            };
        }

        private void AplicarActualizacion(Usuario usuario, UsuarioActualizacionDto dto)
        {
            usuario.Nombres = Limpiar(dto.Nombres);
            usuario.ApellidoPaterno = Limpiar(dto.ApellidoPaterno);
            usuario.ApellidoMaterno = dto.ApellidoMaterno?.Trim();
            usuario.Ci = Limpiar(dto.Ci);
            usuario.CiExtencion = LimpiarMayus(dto.CiExtencion);
            usuario.Telefono = Limpiar(dto.Telefono);
            usuario.Email = Limpiar(dto.Email);
            usuario.UserName = Limpiar(dto.UserName);
            usuario.Role = Limpiar(dto.Role);
            usuario.Activo = dto.Activo;
            usuario.MustChangePassword = dto.MustChangePassword;
        }

        private UsuarioDto? ObtenerYMapear(Func<Usuario?> obtenerUsuario)
        {
            Usuario? usuario = obtenerUsuario();
            return usuario == null ? null : MapearDto(usuario);
        }

        private UsuarioToken? ObtenerTokenActivacionValido(string token)
        {
            token = Limpiar(token);

            if (string.IsNullOrWhiteSpace(token))
                return null;

            return _usuarioTokenService.ValidarToken(
                token,
                TipoTokenConstantes.ActivacionCuenta
            );
        }

        private UsuarioDto MapearDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                Ci = usuario.Ci,
                CiExtencion = usuario.CiExtencion,
                Telefono = usuario.Telefono,
                Activo = usuario.Activo,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Role = usuario.Role,
                MustChangePassword = usuario.MustChangePassword
            };
        }

        private static string Limpiar(string? valor)
        {
            return valor?.Trim() ?? string.Empty;
        }

        private static string LimpiarMayus(string? valor)
        {
            return valor?.Trim().ToUpper() ?? string.Empty;
        }

        public Result ActualizarUsuarioEdicion(UsuarioEdicionDto dto, int? idUsuarioSesion)
        {
            if (dto.IdUsuario <= 0)
                return Result.Fail("El id del usuario no es válido.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Result.Fail("El email es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Role))
                return Result.Fail("El rol es obligatorio.");

            Usuario? usuarioActual = _repository.GetById(dto.IdUsuario);
            if (usuarioActual == null)
                return Result.Fail("El usuario no existe.");

            usuarioActual.Email = dto.Email.Trim();
            usuarioActual.Role = dto.Role.Trim();
            usuarioActual.Activo = dto.Activo;

            int filasAfectadas = _repository.UpdateDatosEdicion(usuarioActual, idUsuarioSesion);

            return filasAfectadas > 0
                ? Result.Ok()
                : Result.Fail("No se pudo actualizar el usuario.");
                }

    }
}
