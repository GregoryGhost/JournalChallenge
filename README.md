# JournalChallenge

JournalChallenge is an ASP.NET Core 8 REST API designed to manage multiple independent hierarchical tree structures and maintain a comprehensive exception journal. Built with Clean Architecture principles and Entity Framework Core (PostgreSQL), the system ensures data integrity through tree-level isolation and sibling uniqueness constraints. It features a robust global exception handling middleware that logs every failure with detailed request context and unique event tracking, responding with standardized error payloads as per technical specifications.

## How to run
Open a terminal in the root solution folder. Run the docker compose file:
>docker compose --env-file ./Configs/Envs/dev.env build

>docker compose --env-file ./Configs/Envs/dev.env up

## How to run migrations
You have to run this commands from the JournalChallenge solution folder.

## Run migrations for the main service
> dotnet ef migrations add InitialCreate --projectJournalChallenge.MigrationService --startup-project JournalChallenge.MigrationService --output-dir Migrations
