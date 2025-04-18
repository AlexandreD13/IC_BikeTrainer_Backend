using Microsoft.EntityFrameworkCore;
using IC_BikeTrainer_Backend.Models;

namespace IC_BikeTrainer_Backend.Repositories
{
    public class Context : DbContext, IContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
    
        public DbSet<User> UsersTable { get; set; }
        
        public DbSet<KnownDevice> DevicesTable { get; set; }
        
        public DbSet<TrainingSession> TrainingSessionsTable { get; set; }

        public DbSet<TrainingEntry> TrainingEntriesTable { get; set; }

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