using IC_BikeTrainer_Backend.Models;

namespace IC_BikeTrainer_Backend.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateUserAsync(string username, string password);
        Task<int> RegisterAsync(RegisterRequest request);
        Task<int> UpdateUserAsync(string username, UpdateUserRequest request);
        Task<bool> UserExistsByUsernameAsync(string username);
        Task<bool> UserExistsByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<List<User>> GetAllUsersAsync();
    }
}