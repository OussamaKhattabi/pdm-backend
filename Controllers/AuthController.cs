using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using pdm.Data;
using pdm.Models;

namespace pdm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {   
        private readonly IConfiguration _configuration;
        private readonly PDMContext _context;

        public AuthController(IConfiguration configuration, PDMContext context)
        {
            _configuration = configuration;

            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is empty");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return BadRequest("Email is already in use");
            }

            bool isUserTableEmpty = !_context.Users.Any();

            // Check if roles "Admin" and "Membre" exist, create them if not
            EnsureRolesExist();

            request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Firstname = request.Firstname, // MODIFTODO 
                Lastname = request.Lastname,
                Email = request.Email,
                Password = request.Password,
                RoleID = isUserTableEmpty ? 1 : 2
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLogin request)
        {
            if (string.IsNullOrEmpty(request.Email)) {
                return BadRequest("User not found");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password)){
                return BadRequest("Wrong password");
            }

            string token = CreateToken(user);

            // Set the token in a cookie
            Response.Cookies.Append("Authorization", $"Bearer {token}", new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true if your site is served over HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(1) // Adjust the expiration as needed
            });
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Firstname),
                new Claim(ClaimTypes.Role, user.RoleID == 1 ? "Admin" : "Membre")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                ); 

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void EnsureRolesExist()
        {
            // Check if roles "Admin" and "Membre" exist, create them if not
            if (!_context.Role.Any(r => r.Name == "Admin"))
            {
                _context.Role.Add(new Role { Name = "Admin", Description = "Admin" });
            }

            if (!_context.Role.Any(r => r.Name == "Membre"))
            {
                _context.Role.Add(new Role { Name = "Membre", Description = "Membre" });
            }

            // Save changes to the database
            _context.SaveChanges();
        }
    }
}
