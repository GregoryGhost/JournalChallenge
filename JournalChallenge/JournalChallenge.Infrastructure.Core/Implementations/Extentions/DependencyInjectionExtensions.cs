namespace JournalChallenge.Infrastructure.Core.Implementations.Extentions;

using FluentValidation;

using JournalChallenge.Application.Core.Abstractions.Messaging;
using JournalChallenge.Infrastructure.Core.Abstractions.Behaviors;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationBasement(this IServiceCollection services)
    {
        var assemblies = new[]
        {
            typeof(DependencyInjection).Assembly,
            typeof(JournalChallenge.Application.ApplicationAbstraction).Assembly
        };

        services.Scan(scan => scan.FromAssemblies(assemblies)
                                  .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                                  .AsImplementedInterfaces()
                                  .WithScopedLifetime()
                                  .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                                  .AsImplementedInterfaces()
                                  .WithScopedLifetime()
                                  .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                                  .AsImplementedInterfaces()
                                  .WithScopedLifetime());

        services.TryDecorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandBaseHandler<>));
        
        services.AddValidatorsFromAssembly(typeof(JournalChallenge.Application.ApplicationAbstraction).Assembly, includeInternalTypes: true);

        services.AddScoped<IRestCustomResultsHandler, CustomResultsHandler>();

        return services;
    }
    
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(DatabaseExtensions.GetDbConnectionString(configuration));

        return services;
    }
}
