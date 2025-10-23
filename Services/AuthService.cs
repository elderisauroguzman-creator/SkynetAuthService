using AuthService.Data;
using AuthService.DTOs;
using AuthService.Interfaces;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AuthDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            var user = await _context.Usuarios.Where(u => u.UserName == loginDto.UserName)
               .Include(u => u.Rol)
                .FirstOrDefaultAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Contrasena))
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
                UserName = user.UserName,
                Rol = user.Rol.Nombre,
                Expiration = DateTime.UtcNow.AddHours(2)
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerDto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.UserName == registerDto.UserName))
                throw new Exception("El usuario ya existe");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new Usuario
            {
                Nombre = registerDto.Nombre,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                IdRolUsuario = registerDto.IdRolUsuario,
                Contrasena = hashedPassword,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            var rol = await _context.RolUsuario.FindAsync(user.IdRolUsuario);

            return new RegisterResponseDto
            {
                Id = user.Id,
                Nombre = user.Nombre,
                UserName = user.UserName,
                Email = user.Email,
                Rol = rol?.Nombre
            };
        }

        private string GenerateJwtToken(Usuario user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("role", user.Rol.Nombre),
                new Claim("id", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
