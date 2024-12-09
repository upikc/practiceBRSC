using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practiceAPI.Models;
using static practiceAPI.Models.practiceContex;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new Usersdata
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role_id = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailsView>>> GetUserDetails()
        {
            IEnumerable<UserDetailsView> userDetails = await _context.UserDetailsView.ToListAsync(); //затуп
            return Ok(userDetails);
        }



        //[HttpGet("login")]
        //public async Task<IActionResult> Login(string email, string password)
        //{
        //    var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

        //    if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        //    {
        //        return Unauthorized();
        //    }

        //    var token = _tokenService.GenerateToken(user);
        //    return Ok(new { Token = token });
        //}

    }

}
