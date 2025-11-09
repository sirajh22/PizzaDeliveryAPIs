using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIzzaDeliveryAPI.Models;
using PIzzaDeliveryAPI.Data;
using PIzzaDeliveryAPI.DTOs;

namespace PIzzaDeliveryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        //get:api/user
        [HttpGet]
        public async Task<IActionResult>GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        //get:api/user/2(get one user)
        [HttpGet("{id}")]
        public async Task<IActionResult>GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("invalid used id");
            return Ok(user);
        }
        //post:api/user
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email
            };
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var response = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            return CreatedAtAction(nameof(GetUsers),new { id = user.Id }, response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateUser(int id,UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;
            await _context.SaveChangesAsync();
            return Ok("Successfully Updated");

        }
        //delete:api/user
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
