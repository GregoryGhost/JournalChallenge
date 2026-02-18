namespace JournalChallenge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
               .AddServices()
               .AddDbContextWithEnvDbConnectionString<AppDbContext>(configuration)
               .AddHealthChecks(configuration)
               .AddApplicationBasement();;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}