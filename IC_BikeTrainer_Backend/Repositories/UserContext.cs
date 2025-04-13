using Microsoft.EntityFrameworkCore;
using IC_BikeTrainer_Backend.Models;

namespace IC_BikeTrainer_Backend.Repositories
{
    public class UserContext : DbContext, IUserContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    
        public DbSet<User> UsersTable { get; set; }
    
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await UsersTable.FirstOrDefaultAsync(u => u.Username == username);
        }
        
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}