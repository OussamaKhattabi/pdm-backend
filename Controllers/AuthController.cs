using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PremiumDeluxeMotorSports_v1.Data;
using PremiumDeluxeMotorSports_v1.Models;
using PremiumDeluxeMotorSports_v1.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PremiumDeluxeMotorSports_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {   
        private readonly IConfiguration _configuration;
        private readonly PremiumDeluxeMotorSports_v1Context _context;

        public AuthController(IConfiguration configuration, PremiumDeluxeMotorSports_v1Context context)
        {
            _configuration = configuration;

            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User request)
        {
            if (string.IsNullOrEmpty(request.UserEmail))
            {
                return BadRequest("Email is empty");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == request.UserEmail);

            if (existingUser != null)
            {
                return BadRequest("Email is already in use");
            }

            bool isUserTableEmpty = !_context.Users.Any();

            // Check if roles "Admin" and "Membre" exist, create them if not
            EnsureRolesExist();

            request.UserPassword = BCrypt.Net.BCrypt.HashPassword(request.UserPassword);

            var newUser = new User
            {
                UserFirstName = request.UserFirstName, // MODIFTODO 
                UserLastName = request.UserLastName,
                UserEmail = request.UserEmail,
                UserPassword = request.UserPassword,
                RoleID = isUserTableEmpty ? 1 : 2
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLogin request)
        {
            if (string.IsNullOrEmpty(request.UserEmail)) {
                return BadRequest("User not found");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == request.UserEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.UserPassword)){
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
                new Claim(ClaimTypes.Name, user.UserFirstName),
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
            if (!_context.Role.Any(r => r.RoleName == "Admin"))
            {
                _context.Role.Add(new Role { RoleName = "Admin", RoleDescription = "Admin" });
            }

            if (!_context.Role.Any(r => r.RoleName == "Membre"))
            {
                _context.Role.Add(new Role { RoleName = "Membre", RoleDescription = "Membre" });
            }

            // Save changes to the database
            _context.SaveChanges();
        }
    }
}
