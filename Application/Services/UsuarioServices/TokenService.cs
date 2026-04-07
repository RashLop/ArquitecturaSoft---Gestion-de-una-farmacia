using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProyectoArqSoft.Domain.Models;

namespace ProyectoArqSoft.Services
{
    public class TokenService : ITokenService
    {
        public string GenerarToken(Usuario usuario, out int expiraEn)
        {
            string secretKey = Environment.GetEnvironmentVariable("JWT_KEY")!;
            string issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
            string audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;
            expiraEn = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE_MINUTES")!);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiraEn),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}