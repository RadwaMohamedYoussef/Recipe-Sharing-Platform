using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recipe_Sharing_Platform.Data;
using Recipe_Sharing_Platform.DTOs;
using Recipe_Sharing_Platform.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Recipe_Sharing_Platform.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var existingUserByEmail = await _context.Users.SingleOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUserByEmail != null)
            {
                throw new Exception("Email is already registered.");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                IsChef = registerDto.IsChef
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully.";
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null)
                {
                    throw new Exception("Invalid Email or password.");
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                    throw new Exception("Invalid Email or password.");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("IsChef", user.IsChef.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Issuer = _configuration["Jwt:Issuer"],      
                    Audience = _configuration["Jwt:Audience"], 
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
