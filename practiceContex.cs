using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using practiceAPI.Models;

namespace practiceAPI
{
    public class practiceContex : DbContext
    {
        public practiceContex(DbContextOptions<practiceContex> options) : base(options) { }
        public DbSet<Usersdata> Users { get; set; }
        public DbSet<ExchangeUserModels> UserDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExchangeUserModels>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToView("UserDetailsView");
            });
        }
        public interface ITokenService
        {
            string GenerateToken(Usersdata user);
        }

        public class TokenService : ITokenService
        {
            private readonly IConfiguration _configuration;

            public TokenService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string GenerateToken(Usersdata user)
            {
                var claims = new[]//данные токена
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),                 //почта
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  //идентификатор токена
                    new Claim(ClaimTypes.Role, user.Role_id.ToString()),                // роль
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));  //создаем ключ используя данные appsettings.json
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);                 //создание чегото через алгоритм HmacSha256

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],       //оказалось нужным
                    audience: _configuration["Jwt:Audience"],   //оказалось нужным
                    claims: claims,                                                                             // данные ранее засунутые в Claim
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),    // время действия токена
                    signingCredentials: creds);                                                                 // преобразованый в чтото ключ

                return new JwtSecurityTokenHandler().WriteToken(token);     //WriteToken преобразует метод в строку
            }
        }
    }
}
