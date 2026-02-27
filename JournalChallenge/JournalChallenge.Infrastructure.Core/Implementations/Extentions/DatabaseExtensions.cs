namespace JournalChallenge.Infrastructure.Core.Implementations.Extentions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DatabaseExtensions
    {
        private const string DB_CONNECTION_STRING = "DB_CONNECTION_STRING";

        public static IServiceCollection AddDbContextWithEnvDbConnectionString<TContext, TContextImplementation>(
            this IServiceCollection services, IConfiguration configuration)
            where TContextImplementation : DbContext, TContext
        {
            var connectionString = GetDbConnectionString(configuration);

            services.AddDbContext<TContext, TContextImplementation>(optionsBuilder => optionsBuilder.UseNpgsql(connectionString));

            return services;
        }

        public static string GetDbConnectionString(IConfiguration configuration)
        {
            var connectionString = GetDbConnectionStringFromEnv()
                                   ?? configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new ApplicationException("You should provide connection string to database.");

            return connectionString;
        }

        public static string? GetDbConnectionStringFromEnv()
        {
            return Environment.GetEnvironmentVariable(DB_CONNECTION_STRING);
        }
    }
}