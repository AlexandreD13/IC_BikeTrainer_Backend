using Microsoft.EntityFrameworkCore;
using IC_BikeTrainer_Backend.Models;
using IC_BikeTrainer_Backend.Repositories;

namespace IC_BikeTrainer_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IContext _context;
    
        public UserService(IContext context)
        {
            _context = context;
        }
    
        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _context.GetByUsernameAsync(username);
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
    
            await _context.UsersTable.AddAsync(user);
            await _context.SaveChangesAsync();
    
            return user.Id;
        }
    
        public async Task<int> UpdateUserAsync(string username, UpdateUserRequest request)
        {
            var user = await _context.UsersTable.FirstOrDefaultAsync(u => u.Username == username);
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
    
            return await _context.SaveChangesAsync();
        }
    
        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            return await _context.UsersTable.AnyAsync(u => u.Username == username);
        }
    
        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.UsersTable.AnyAsync(u => u.Email == email);
        }
        
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.GetByUsernameAsync(username);
        }
        
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.UsersTable.ToListAsync();
        }
        
        public async Task DeleteUserAsync(string username)
        {
            try
            {
                var user = await _context.GetByUsernameAsync(username);

                if (user == null)
                    throw new InvalidOperationException("User not found.");

                _context.UsersTable.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting user: " + ex.Message);
            }
        }

        public async Task DeleteAllUsersAsync()
        {
            try
            {
                var users = await _context.UsersTable.ToListAsync();
                
                if (users.Count == 0)
                    throw new InvalidOperationException("No users found to delete.");

                _context.UsersTable.RemoveRange(users);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting all users: " + ex.Message);
            }
        }
    }
}
