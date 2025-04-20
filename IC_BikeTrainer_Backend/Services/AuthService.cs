using IC_BikeTrainer_Backend.Models;
using IC_BikeTrainer_Backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IC_BikeTrainer_Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IContext _context;
    
        public AuthService(IContext context)
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
    }
}