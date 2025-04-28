using Recipe_Sharing_Platform.DTOs;

namespace Recipe_Sharing_Platform.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
