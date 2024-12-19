using Microsoft.AspNetCore.Mvc;
using PlantPalAPI.Data;
using PlantPalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;

namespace PlantPalAPI.Controllers
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
        public async Task<IActionResult> Register(User user)
        {
           
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Bu email adresiyle kayıtlı bir kullanıcı zaten mevcut." });
            }

            
            user.Password = HashPassword(user.Password);

            // Kullanıcıyı ekle
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            if (result <= 0)
            {
                return StatusCode(500, new { message = "Kayıt sırasında bir hata oluştu." });
            }
            return Ok(new { message = "Kayıt başarılı" });
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null || !VerifyPassword(loginRequest.Password, user.Password))
            {
                return Unauthorized(new { message = "Geçersiz giriş bilgileri." });
            }

            return Ok(new { message = "Giriş başarılı", userId = user.Id });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            if (!Request.Headers.TryGetValue("User-ID", out var userIdHeader) || !int.TryParse(userIdHeader, out int userId))
            {
                return Unauthorized(new { message = "Geçersiz Kullanıcı ID'si." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return Unauthorized(new { message = "Kullanıcı bulunamadı." });
            }

            var users = await _context.Users.Select(u => new { u.Id, u.Name, u.Email }).ToListAsync();
            return Ok(users);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var salt = Guid.NewGuid().ToString();
            var saltedPassword = salt + password;
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashedBytes) + ":" + salt;
        }

        // Şifreyi doğrula
        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            var parts = storedPassword.Split(':');
            if (parts.Length != 2) return false;

            var hashedPassword = parts[0];
            var salt = parts[1];
            var saltedInputPassword = salt + inputPassword;

            using var sha256 = SHA256.Create();
            var hashedInputBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedInputPassword));
            var hashedInputPassword = Convert.ToBase64String(hashedInputBytes);

            return hashedPassword == hashedInputPassword;
        }
    }
}
