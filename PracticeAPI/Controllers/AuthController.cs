using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practiceAPI.Models;
using static practiceAPI.practiceContex;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace practiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly practiceContex _context;
        private readonly ITokenService _tokenService;

        public AuthController(practiceContex context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        /// <response code="200">Успешная регистрация. Возвращает токен доступа</response>
        /// <response code="409">Ошибка. Почта занята</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new Usersdata
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role_id = 3
            };

            if (_context.Users.FirstOrDefault(x => x.Email == model.Email) != default)
                return Conflict("Почта занята");


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDetails = await _context.UserDetails.SingleOrDefaultAsync(x => x.email == model.Email);

            var token = _tokenService.GenerateToken(user);
            return Ok(new
            {
                Token = token,
                User = userDetails
            });
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <returns>Токен доступа</returns>
        /// <response code="200">Успешый вход. Возвращает токен доступа</response>
        /// <response code="400">Ошибка. Не верные данные</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return BadRequest();
            }
            var userDetails = await _context.UserDetails.SingleOrDefaultAsync(x => x.email == model.Email);

            var token = _tokenService.GenerateToken(user);
            return Ok(new { 
                Token = token,
                User = userDetails
            });
        }
    }

}
