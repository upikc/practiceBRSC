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

        [HttpGet("GetUsersWithDetails")] //USERS
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<IEnumerable<UserDetailsView>>> GetUserDetails()
        {
            var currentUser = GetCurrentUser();
            if (currentUser.Role_id != 1)
            {
                return await _context.UserDetails.Where(u => u.role != "Admin").ToListAsync();//исключить админов
            }
            return await _context.UserDetails.ToListAsync();
        }

        [HttpGet("GetUserById")] //USER BY ID
        [Authorize(Policy = "Manager")]
        public async Task<ActionResult<UserDetailsView>> GetUser(int id)
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
