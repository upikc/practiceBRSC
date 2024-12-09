using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace practiceAPI.Models
{
    public class practiceContex : DbContext
    {
        public practiceContex(DbContextOptions<practiceContex> options) : base(options) { }
        public DbSet<Usersdata> Users { get; set; }
        public DbSet<UserDetailsView> UserDetailsView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //
            modelBuilder.Entity<Usersdata>().HasData(
                new Usersdata { Id = 0, Name = "Admin",
                            Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                            Role_id = 1}
            );
            //
            modelBuilder.Entity<UserDetailsView>(entity =>
            {
                entity.HasNoKey();
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
                    new Claim(ClaimTypes.Role, user.Role_id.ToString())                            //роль
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));  //создаем ключ используя данные appsettings.json
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);                 //создание чегото через алгоритм HmacSha256

                var token = new JwtSecurityToken(
                    claims: claims,                                                                             // данные ранее засунутые в Claim
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),    // время действия токена
                    signingCredentials: creds);                                                                 // преобразованый в чтото ключ

                return new JwtSecurityTokenHandler().WriteToken(token);     //WriteToken преобразует метод в строку
            }
        }
    }
}
