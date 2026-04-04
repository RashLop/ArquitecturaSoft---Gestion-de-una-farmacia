using System.Data;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.FactoryProducts;

namespace ProyectoArqSoft.Services
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

        public Result CrearUsuario(UsuarioRegistroDto dto, string role)
        {
            Result validacionEntrada = _registroValidador.Validar(dto);
            if (!validacionEntrada.IsSuccess)
                return validacionEntrada;

            role = role?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(role))
                return Result.Fail("El rol es obligatorio.");

            Result validacionNegocio = _negocioValidador.ValidarRegistro(dto);
            if (!validacionNegocio.IsSuccess)
                return validacionNegocio;

            string userNameGenerado = dto.UserName.Trim();
            string passwordTemporal = dto.Password;
            string passwordHash = PasswordHelper.Hash(passwordTemporal);
            Usuario usuario = new Usuario
            {
                Nombres = dto.Nombres.Trim(),
                ApellidoPaterno = dto.ApellidoPaterno.Trim(),
                ApellidoMaterno = dto.ApellidoMaterno.Trim(),
                Ci = dto.Ci.Trim(),
                CiExtencion = dto.CiExtencion.Trim().ToUpper(),
                Telefono = dto.Telefono.Trim(),
                Email = dto.Email.Trim(),
                UserName = userNameGenerado,
                PasswordHash = passwordHash,
                Role = role,
                Activo = 1,
                MustChangePassword = 1
            };

            int filasAfectadas = _repository.Insert(usuario);

            if (filasAfectadas <= 0)
                return Result.Fail("No se pudo registrar el usuario.");

            Usuario? usuarioRegistrado = _repository.GetByEmail(usuario.Email);
            if (usuarioRegistrado == null)
                return Result.Fail("El usuario fue registrado, pero no se pudo recuperar su información.");

            UsuarioTokenGeneracionDto tokenDto = new UsuarioTokenGeneracionDto
            {
                IdUsuario = usuarioRegistrado.IdUsuario,
                TipoToken = TipoTokenConstantes.ActivacionCuenta,
                MinutosExpiracion = 60
            };

            Result validacionToken = _usuarioTokenService.GenerarToken(tokenDto, out string tokenPlano);
            if (!validacionToken.IsSuccess)
                return validacionToken;

            string tokenSeguro = Uri.EscapeDataString(tokenPlano);
            string enlaceActivacion = $"http://localhost:5081/Auth/ActivarCuenta?token={tokenSeguro}";

            Result validacionCorreo = _emailService.EnviarCorreoActivacionCuenta(
                usuarioRegistrado.Email,
                usuarioRegistrado.Nombres,
                usuarioRegistrado.UserName,
                passwordTemporal,
                enlaceActivacion
            );

            if (!validacionCorreo.IsSuccess)
                return validacionCorreo;

            return Result.Ok();
        }

        public Result ActualizarUsuario(UsuarioActualizacionDto dto)
        {
            Result validacionEntrada = _actualizacionValidador.Validar(dto);
            if (!validacionEntrada.IsSuccess)
                return validacionEntrada;

            Result validacionNegocio = _negocioValidador.ValidarActualizacion(dto);
            if (!validacionNegocio.IsSuccess)
                return validacionNegocio;

            Usuario? usuarioActual = _repository.GetById(dto.IdUsuario);
            if (usuarioActual == null)
                return Result.Fail("El usuario no existe.");

            usuarioActual.Nombres = dto.Nombres.Trim();
            usuarioActual.ApellidoPaterno = dto.ApellidoPaterno.Trim();
            usuarioActual.ApellidoMaterno = dto.ApellidoMaterno.Trim();
            usuarioActual.Telefono = dto.Telefono.Trim();
            usuarioActual.Email = dto.Email.Trim();
            usuarioActual.UserName = dto.UserName.Trim();
            usuarioActual.Role = dto.Role.Trim();
            usuarioActual.Activo = dto.Activo;
            usuarioActual.MustChangePassword = dto.MustChangePassword;

            int filasAfectadas = _repository.Update(usuarioActual);

            if (filasAfectadas <= 0)
                return Result.Fail("No se pudo actualizar el usuario.");

            return Result.Ok();
        }

        public Result EliminarUsuario(int idUsuario)
        {
            Result validacionNegocio = _negocioValidador.ValidarEliminacion(idUsuario);
            if (!validacionNegocio.IsSuccess)
                return validacionNegocio;

            Usuario? usuario = _repository.GetById(idUsuario);
            if (usuario == null)
                return Result.Fail("El usuario no existe.");

            int filasAfectadas = _repository.Delete(usuario);

            if (filasAfectadas <= 0)
                return Result.Fail("No se pudo eliminar el usuario.");

            return Result.Ok();
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

        public Result ValidarActivacionCuenta(string token)
        {
            token = token?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(token))
                return Result.Fail("Token inválido.");

            UsuarioToken? tokenValido = _usuarioTokenService.ValidarToken(
                token,
                TipoTokenConstantes.ActivacionCuenta
            );

            if (tokenValido == null)
                return Result.Fail("El token no es válido, ya fue usado o expiró.");

            return Result.Ok();
        }

        public Result ActivarCuenta(string token, string nuevaPassword)
        {
            token = token?.Trim() ?? string.Empty;
            nuevaPassword = nuevaPassword?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(token))
                return Result.Fail("Token inválido.");

            if (string.IsNullOrWhiteSpace(nuevaPassword))
                return Result.Fail("La nueva contraseña es obligatoria.");

            UsuarioToken? tokenValido = _usuarioTokenService.ValidarToken(
                token,
                TipoTokenConstantes.ActivacionCuenta
            );

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

            Result validacionToken = _usuarioTokenService.MarcarComoUsado(tokenValido.IdUsuarioToken);
            if (!validacionToken.IsSuccess)
                return validacionToken;

            Console.WriteLine("TOKEN RECIBIDO: [" + token + "]");
            Console.WriteLine("HASH CALCULADO: [" + TokenHelper.GenerarTokenHash(token) + "]");

            return Result.Ok();
        }
    }
    
}