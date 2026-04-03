using ProyectoArqSoft.DTO;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IValidacion<UsuarioLoginRequestDto> _loginValidador;
        private readonly ITokenService _tokenService;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IValidacion<UsuarioLoginRequestDto> loginValidador,
            ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _loginValidador = loginValidador;
            _tokenService = tokenService;
        }

        public Validacion IniciarSesion(UsuarioLoginRequestDto dto, out UsuarioLoginResponseDto? respuesta)
        {
            respuesta = null;

            Validacion validacionEntrada = _loginValidador.Validar(dto);
            if (!validacionEntrada.IsSuccess)
                return validacionEntrada;

            string emailOUserName = dto.EmailOUserName?.Trim() ?? string.Empty;
            string password = dto.Password ?? string.Empty;

            Usuario? usuario = BuscarPorEmailOUserName(emailOUserName);
            if (usuario == null)
                return Validacion.Fail("Las credenciales son incorrectas.");

            if (usuario.Activo == 0)
                return Validacion.Fail("El usuario se encuentra inactivo.");

            bool passwordValido = PasswordHelper.Verify(password, usuario.PasswordHash);
            if (!passwordValido)
                return Validacion.Fail("Las credenciales son incorrectas.");

            string token = _tokenService.GenerarToken(usuario, out int expiraEn);

            respuesta = new UsuarioLoginResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                UserName = usuario.UserName,
                Role = usuario.Role,
                MustChangePassword = usuario.MustChangePassword == 1,
                Token = token,
                ExpiraEn = expiraEn
            };

            return Validacion.Ok();
        }

        private Usuario? BuscarPorEmailOUserName(string emailOUserName)
        {
            Usuario? usuario = _usuarioRepository.GetByEmail(emailOUserName);

            if (usuario == null)
                usuario = _usuarioRepository.GetByUserName(emailOUserName);

            return usuario;
        }
    }
}