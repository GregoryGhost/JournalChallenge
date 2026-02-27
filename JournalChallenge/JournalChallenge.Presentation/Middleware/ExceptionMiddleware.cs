namespace JournalChallenge.Presentation.Middleware;

using System.Text;
using System.Text.Json;
using JournalChallenge.Application.Abstractions.Data;
using JournalChallenge.Domain.Journal;
using JournalChallenge.Presentation.DTOs;

public class ExceptionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Enable buffering so the body can be read multiple times
        context.Request.EnableBuffering();

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var eventId = DateTime.UtcNow.Ticks;
        
        // Context Capture: Query String
        var queryDict = context.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());
        var queryParams = queryDict.Any() ? JsonSerializer.Serialize(queryDict) : "{}";

        // Context Capture: Body
        var bodyParams = "{}";
        if (context.Request.ContentLength > 0)
        {
            context.Request.Body.Position = 0;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var content = await reader.ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    bodyParams = content;
                }
            }
        }

        // Persistence
        using (var scope = scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            
            var entry = new ExceptionJournal
            {
                EventId = eventId,
                Timestamp = DateTime.UtcNow,
                QueryParams = queryParams,
                BodyParams = bodyParams,
                StackTrace = exception.StackTrace ?? string.Empty,
                ExceptionType = exception.GetType().Name,
                Message = exception.Message
            };

            dbContext.ExceptionJournals.Add(entry);
            await dbContext.SaveChangesAsync();
        }

        // Response Formatting
        context.Response.ContentType = "application/json";
        
        object response;
        if (exception is SecureException secureEx)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            response = new SecureResponseError
            {
                Id = eventId.ToString(),
                Data = new ResponseErrorMessage { Message = secureEx.Message }
            };
        }
        else if (exception is ValidationException validationEx)
        {
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            response = new ValidationResponseError
            {
                Id = eventId.ToString(),
                Data = new ResponseErrorMessage
                { 
                    Message = validationEx.Message,
                    Errors = validationEx.Errors
                }
            };
        }
        else if (exception is KeyNotFoundException notFoundEx)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            response = new NotFoundResponseError
            {
                Id = eventId.ToString(),
                Data = new ResponseErrorMessage { Message = notFoundEx.Message }
            };
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            response = new ExceptionResponseError(eventId);
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
