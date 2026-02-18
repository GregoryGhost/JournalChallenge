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
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
                                  .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                                  .AsImplementedInterfaces()
                                  .WithScopedLifetime()
                                  .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                                  .AsImplementedInterfaces()
                                  .WithScopedLifetime()
                                  .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                                  .AsImplementedInterfaces()
                                  .WithScopedLifetime());

        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationDecorator.CommandBaseHandler<>));

        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandBaseHandler<>));
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

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
