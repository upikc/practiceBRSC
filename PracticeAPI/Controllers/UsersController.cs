using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using practiceAPI.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace practiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly practiceContex _context;

        public UsersController(practiceContex context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение данных пользователей
        /// </summary>
        /// <response code="200">Возвращает данные пользователей</response>
        /// <response code="401">Недостаток токена</response>
        /// <response code="403">Доступ запрещен</response>
        [HttpGet("GetUsersWithDetails")] //USERS
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<ExchangeUserModels>>> GetUserDetails()
        {
            var currentUser = GetCurrentUser();
            if (currentUser.Role_id != 1)
            {
                return await _context.UserDetails.Where(u => u.role != "Admin").ToListAsync();//исключить админов
            }
            return await _context.UserDetails.ToListAsync();
        }

        /// <summary>
        /// Получение данных пользователя по ID
        /// </summary>
        /// <response code="200">Возвращает данные пользователя</response>
        /// <response code="401">Недостаток токена</response>
        /// <response code="403">Доступ запрещен</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpGet("GetUserById")] //USER BY ID
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<ExchangeUserModels>> GetUser(int id)
        {
            var user = await _context.UserDetails.FirstOrDefaultAsync(u => u.id == id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role_id != 1 && user.role == "Admin")//получение админа не админом
            {
                return Forbid();
            }

            return Ok(user);
        }

        /// <summary>
        /// Обновление имени и почты пользователя
        /// </summary>
        /// <response code="200">Обновление успешно</response>
        /// <response code="401">Недостаток токена</response>
        /// <response code="403">Доступ запрещен</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpPut("UpdateUserData")] //Update
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserModel model)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role_id != 1 && user.Role_id == 1)//изминение админа не админом
            {
                return Forbid();
            }

            user.Name = model.Name;
            user.Email = model.Email;

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Обновление имени, почты, пароля и роли пользователя
        /// </summary>
        /// <response code="200">Обновление успешно</response>
        /// <response code="401">Недостаток токена</response>
        /// <response code="403">Доступ запрещен</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpPut("UpdateUserDataWithPassword")] //Update
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateUserWithPass(int id, [FromBody] UpdateUserModelWithPass model)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = model.Name;  //доступно только админу, поэтому нет проверок
            user.Email = model.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            user.Role_id = model.Role_id;

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <response code="200">Удаление успешно</response>
        /// <response code="401">Недостаток токена</response>
        /// <response code="403">Доступ запрещен</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpDelete("DeleteUser")] //Delete
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private Usersdata GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return _context.Users.SingleOrDefault(x => x.Email == email);
        }
    }

}
