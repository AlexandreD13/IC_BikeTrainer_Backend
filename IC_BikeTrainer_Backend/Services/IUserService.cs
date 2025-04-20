using IC_BikeTrainer_Backend.Models;

namespace IC_BikeTrainer_Backend.Services
{
    public interface IUserService
    {
        Task<int> UpdateUserAsync(string username, UpdateUserRequest request);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUserAsync(string username);
        Task DeleteAllUsersAsync();
    }
}