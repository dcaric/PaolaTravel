using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Models;

namespace Travel.API.Controllers
{
    // secure authorization, use JWT
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Use **ActionResult<T>** when you're returning an object, and you still want to have flexibility to return error responses 
         * (NotFound, BadRequest, etc.)
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(int id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);

            if (user == null) return NotFound();
            Console.WriteLine($"User found: {user}");

            return user;
        }

        [HttpGet("me")]
        public async Task<ActionResult<ApplicationUser>> GetCurrentUser()
        {
            Console.WriteLine($"User:: {User}");

            /*NameClaimType = ClaimTypes.Name tells the system
             * "Use the Name claim from JWT as the current user's identity."
             * This directly affects what User.Identity.Name will return.
             * You should match this to whatever claim you're putting in the token for the user's name.
             * THIS IS USED IN Travet.Web / 
             */
            var userName = User.Identity?.Name;

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == userName);
            Console.WriteLine($"User found: {user}");


            if (user == null) return NotFound();

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostUser(ApplicationUser user)
        {
            _context.ApplicationUsers.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        // old way when all data has to arrive from the forntend to backend
        /*
        public async Task<IActionResult> PutUser(int id, ApplicationUser user)
        {
            if (id != user.Id) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */

        // new way to update user
        public async Task<IActionResult> PutUser(int id, [FromBody] UpdateUserRequest request)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null) return NotFound();

            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            await _context.SaveChangesAsync();
            Console.WriteLine($"Updating user with ID {id} by {User.Identity?.Name}");

            return NoContent();
        }



        // route for the curent user, the one logged
        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserRequest request)
        {
            var userName = User.Identity?.Name;
            Console.WriteLine($"Updating user with {userName}");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null) return NotFound();

            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null) return NotFound();

            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
