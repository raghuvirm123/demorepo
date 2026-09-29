using LoginRegisterAPI.Data;
using LoginRegisterAPI.DTOs;
using LoginRegisterAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace LoginRegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;


        public AuthController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (existingUser != null)
            {
                return BadRequest(new
                {
                    message = "Email already exist"
                });

            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                CraeteDate = DateTime.Now,
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registered Successfully"
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            // Check password
            bool passwordValid = VerifyPassword(
                model.Password,
                user.Password
            );

            if (!passwordValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            return Ok(new
            {
                message = "Login successful",
                userId = user.Id,
                name = user.Name,
                email = user.Email
            });
        }

        //// Password Hash
        //private string HashPassword(string password)
        //{
        //    using var sha256 = SHA256.Create();

        //    var bytes = Encoding.UTF8.GetBytes(password);

        //    var hash = sha256.ComputeHash(bytes);

        //    return Convert.ToBase64String(hash);
        //}

        // Verify Password
        private bool VerifyPassword(string password, string storedPassword)
        {
            // var hashedPassword = HashPassword(password);

            // return hashedPassword == storedPassword;

            return password == storedPassword;
        }

    }
}
