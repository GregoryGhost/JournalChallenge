namespace JournalChallenge.Infrastructure.Core.Implementations.Extentions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DatabaseExtensions
    {
        private const string DB_CONNECTION_STRING = "DB_CONNECTION_STRING";

        public static IServiceCollection AddDbContextWithEnvDbConnectionString<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            var connectionString = GetDbConnectionString(configuration);
    
            services.AddDbContext<TContext>(optionsBuilder => optionsBuilder.UseNpgsql(connectionString));

            return services;
        }

        public static string? GetDbConnectionStringFromEnv()
        {
            return Environment.GetEnvironmentVariable(DB_CONNECTION_STRING);
        }

        public static string GetDbConnectionString(IConfiguration configuration)
        {
            var connectionString = GetDbConnectionStringFromEnv()
                                   ?? configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new ApplicationException("You should provide connection string to database.");
            
            return connectionString;
        }
    }
}