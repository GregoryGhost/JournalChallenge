# JournalChallenge

JournalChallenge is an ASP.NET Core 8 REST API designed to manage multiple independent hierarchical tree structures and maintain a comprehensive exception journal. Built with Clean Architecture principles and Entity Framework Core (PostgreSQL), the system ensures data integrity through tree-level isolation and sibling uniqueness constraints. It features a robust global exception handling middleware that logs every failure with detailed request context and unique event tracking, responding with standardized error payloads as per technical specifications.

## How to run
Open a terminal in the `JournalChallenge/` directory. Run the following command to start the application:

```bash
docker compose up --build
```

The API will be available at `http://localhost:5000`.

## How to run integration tests
The project uses a containerized **Postman/Newman** test suite to verify business rules, recursive operations, and the exception journaling system.

### Option 1: Using PowerShell (Recommended)
From the root directory or `JournalChallenge/` directory, run the automated script:

```powershell
.\JournalChallenge\run-integration-tests.ps1
```

This script handles the full lifecycle: environment cleanup, infrastructure startup (DB/Migrate/API), health checks, test execution, and final teardown.

### Option 2: Using Docker Compose Manual Command
Run the following command in the `JournalChallenge/` directory:

```bash
docker compose -f docker.testing.yaml up --build --exit-code-from newman
```

This command will:
1.  Spin up the API and PostgreSQL containers with pinned versions.
2.  Wait for the `/health` endpoint to be ready.
3.  Execute the Postman collections using Newman.
4.  Automatically stop all containers and return the test exit code.

### Swagger UI
After the application is running, you can access the Swagger UI for API documentation and testing at:
[http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

## How to run migrations
To apply or manage database migrations, you can use the `dotnet ef` CLI from the `JournalChallenge/JournalChallenge.Infrastructure` directory:

```bash
# To apply migrations
dotnet ef database update --project JournalChallenge.Infrastructure --startup-project JournalChallenge.Presentation
```
