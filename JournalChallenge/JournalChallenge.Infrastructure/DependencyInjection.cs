namespace JournalChallenge.Infrastructure;

using JournalChallenge.Application.Abstractions.Data;
using JournalChallenge.Infrastructure.Core.Implementations.Extentions;
using JournalChallenge.Infrastructure.Database;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
               .AddServices()
               .AddDbContextWithEnvDbConnectionString<AppDbContext>(configuration)
               .AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<AppDbContext>())
               .AddHealthChecks(configuration)
               .AddApplicationBasement();;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}