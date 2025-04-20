using IC_BikeTrainer_Backend.Models;

namespace IC_BikeTrainer_Backend.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateUserAsync(string username, string password);
        Task<int> RegisterAsync(RegisterRequest request);
        Task<bool> UserExistsByUsernameAsync(string username);
        Task<bool> UserExistsByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}