using Microsoft.EntityFrameworkCore;
using IC_BikeTrainer_Backend.Repositories;
using IC_BikeTrainer_Backend.Services;

namespace IC_BikeTrainer_Backend.Configuration
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Get database path from configuration
            string dbPath = configuration["Database:Path"] ?? "Data/database.db";
            string dbDirectory = Path.GetDirectoryName(dbPath) ?? Directory.GetCurrentDirectory();

            // Ensure the database directory exists
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }

            // Register DbContext
            services.AddDbContext<UserContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // Register application services
            services.AddScoped<IUserService, UserService>();
        }
    }
}