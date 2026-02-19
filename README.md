# JournalChallenge

JournalChallenge is an ASP.NET Core 8 REST API designed to manage multiple independent hierarchical tree structures and maintain a comprehensive exception journal. Built with Clean Architecture principles and Entity Framework Core (PostgreSQL), the system ensures data integrity through tree-level isolation and sibling uniqueness constraints. It features a robust global exception handling middleware that logs every failure with detailed request context and unique event tracking, responding with standardized error payloads as per technical specifications.

## How to run
Open a terminal in the `JournalChallenge/` directory. Run the following command to start the application:

```bash
docker compose up --build
```

The API will be available at `http://localhost:5000`.

### Swagger UI
After the application is running, you can access the Swagger UI for API documentation and testing at:
[http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

## How to run migrations
To apply or manage database migrations, you can use the `dotnet ef` CLI from the `JournalChallenge/JournalChallenge.Infrastructure` directory:

```bash
# To apply migrations
dotnet ef database update --project JournalChallenge.Infrastructure --startup-project JournalChallenge.Presentation
```
