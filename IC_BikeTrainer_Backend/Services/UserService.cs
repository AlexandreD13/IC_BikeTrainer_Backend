using Microsoft.EntityFrameworkCore;
using IC_BikeTrainer_Backend.Models;
using IC_BikeTrainer_Backend.Repositories;

namespace IC_BikeTrainer_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _userContext;
    
        public UserService(UserContext userContext)
        {
            _userContext = userContext;
        }
    
        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userContext.GetByUsernameAsync(username);
            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            return user;
        }
    
        public async Task<int> RegisterAsync(RegisterRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Password = request.Password,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Email = request.Email,
                Weight = request.Weight,
                Height = request.Height
            };
    
            await _userContext.UsersTable.AddAsync(user);
            await _userContext.SaveChangesAsync();
    
            return user.Id;
        }
    
        public async Task<int> UpdateUserAsync(string username, UpdateUserRequest request)
        {
            var user = await _userContext.UsersTable.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return 0;
            }
    
            if (!string.IsNullOrEmpty(request.Firstname))
                user.Firstname = request.Firstname;
            if (!string.IsNullOrEmpty(request.Lastname))
                user.Lastname = request.Lastname;
            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;
            if (request.Weight.HasValue)
                user.Weight = request.Weight;
            if (request.Height.HasValue)
                user.Height = request.Height;
    
            return await _userContext.SaveChangesAsync();
        }
    
        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            return await _userContext.UsersTable.AnyAsync(u => u.Username == username);
        }
    
        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _userContext.UsersTable.AnyAsync(u => u.Email == email);
        }
        
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userContext.GetByUsernameAsync(username);
        }
        
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userContext.UsersTable.ToListAsync();
        }
    }
}
