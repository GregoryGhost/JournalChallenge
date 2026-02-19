namespace JournalChallenge.Presentation;

using Microsoft.AspNetCore.Mvc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddControllers()
                .ConfigureApiBehaviorOptions(
                    options =>
                    {
                        options.InvalidModelStateResponseFactory = context =>
                        {
                            var problemDetails = new ValidationProblemDetails(context.ModelState)
                            {
                                Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
                                Title = "One or more validation errors occurred.",
                                Status = StatusCodes.Status422UnprocessableEntity,
                                Detail = "See the errors property for details.",
                                Instance = context.HttpContext.Request.Path
                            };

                            return new UnprocessableEntityObjectResult(problemDetails);
                        };
                    });

        return services;
    }
}