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

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<Usersdata>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Usersdata>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role_id != 1 && currentUser.Id != id)
            {
                return Forbid();
            }

            return user;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserModel model)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUser = GetCurrentUser();
            if (currentUser.Role_id != 1 && currentUser.Id != id)
            {
                return Forbid();
            }

            user.Name = model.Name;
            user.Email = model.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
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
            return NoContent();
        }

        private Usersdata GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return _context.Users.SingleOrDefault(x => x.Email == email);
        }
    }

}
