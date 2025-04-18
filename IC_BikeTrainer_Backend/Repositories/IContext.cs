using IC_BikeTrainer_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace IC_BikeTrainer_Backend.Repositories;

public interface IContext
{
    DbSet<User> UsersTable { get; set; }
    Task<User?> GetByUsernameAsync(string username);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);}