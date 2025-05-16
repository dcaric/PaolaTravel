using Microsoft.AspNetCore.Mvc;
using Travel.API.Data;
using Travel.API.Helpers;
using Travel.API.Models;
using Travel.API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Travel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthController(ApplicationDbContext context, JwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /*
         * Use **IActionResult** when you just return status codes or small info, not a data model
         * 
         * Why use a DTO (RegisterRequest) instead of the entity?
            Very important:

            Direct ApplicationUser	DTO (RegisterRequest)
            Risky: exposes all fields of the model	Safe: only the fields you want from the client
            Tightly coupled to DB schema	Decoupled, flexible
            Could accidentally allow bad fields (e.g., Id, CreatedAt, Token)	You control exactly what comes in
            That's why DTOs (Data Transfer Objects) are the modern, safe way to handle request/response data.

            Aspect	            PostUser(ApplicationUser user)          Register([FromBody] RegisterRequest request)
            Return type	        ActionResult<ApplicationUser>	        IActionResult
            Return value	    Sends back created user	                Sends status, maybe token
            Input type	        Entity (ApplicationUser)	            DTO (RegisterRequest)
            [FromBody] needed?	Optional	                            Recommended
            Use case	        CRUD (EF-based scaffolding)	            Custom logic (auth, validation)
            Secure/flexible     Risky if exposed directly	            Best practice

         */
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            Console.WriteLine($"User IsAdmin: {request.IsAdmin}");

            // fetches using _context.ApplicationUsers all users and check Email with received request.Email)
            // if Email is found it means user which wants to register with that Email already exists, mail is used
            if (_context.ApplicationUsers.Any(u => u.Email == request.Email))
                return BadRequest("Email already exists");


            // it proceeds here is above chec is passed
            // here builds object user based on all input received from request (from the form where user inserted data)
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                IsAdmin = request.IsAdmin
            };

            // now ads that new user using _context (which is connection with a SQL db), cretes ApplicationUsers object from previously
            // filled object user
            _context.ApplicationUsers.Add(user);
            // calls function SaveChangesAsync to save the _context and waits to complete
            await _context.SaveChangesAsync();

            // return 200 OK to the frontend (Swagger)
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            Console.WriteLine($"User: {request.UserName}");

            // check via SQL db using context is the user with that name stored in request.UserName (this is what user writes in the web form)
            // exists? 
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == request.UserName);


            // is user does not exists it returns this down
            if (user == null)
            {
                Console.WriteLine("User not found");
                return Unauthorized();
            }

            // id user exists thsi will be written, it is just for debugging
            Console.WriteLine($"User found: {user.UserName}");
            Console.WriteLine($"Incoming password: {request.Password}");
            Console.WriteLine($"Stored hash: {user.PasswordHash}");
            Console.WriteLine($"Password match: {BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)}");

            // here is checked pasword written in the web form with the saved password in the SQL db
            // it uses class BCrypt to dercypt saved password, because password are not saved in plain text but encrypted
            // and this is decrypting, and if does not match it returns error
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized();


            // if password validation passed code comes here and it is returnd to teh fronend (Swagger)
            var token = _jwtTokenGenerator.Generate(user);
            Console.WriteLine($"BACKEND token: {token}");

            return Ok(new { token });
        }
    }
}