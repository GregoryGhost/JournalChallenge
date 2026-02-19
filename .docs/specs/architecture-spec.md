# Architecture Specification

## 1. Architectural Principles
- **Architecture**: Clean Architecture.
- **Framework**: ASP.NET Core 8
- **Database**: PostgreSQL (Code-First approach)
- **ORM**: Entity Framework Core

## 2. Architectural Conventions

### 2.1 Feature-per-Folder Organization
The project strictly follows a **Feature-per-Folder** structure to ensure high cohesion and maintainability:
- **Application Layer**: Queries, Commands, their respective Handlers, and associated Validators must be co-located within a single folder named after the specific feature (e.g., `Journal/GetTree/`).
- **Test Layer**: Unit and validation tests must mirror this organizational structure, ensuring that tests for a specific feature are grouped together in a corresponding directory (e.g., `Journal/GetTree/GetTreeQueryValidationTests.cs`).

## 3. Testing
- **NUnit 4.4.0** (Testing framework)
- **FluentAssertions 8.8.0** (For readable test assertions)
- **Microsoft.EntityFrameworkCore.InMemory 8.0.24** (For in-memory database testing)
- **Test Naming Convention for handlers**: Use `Test<Scenario>Should<Expectation>` (e.g., `TestLoginUserShouldBeSuccess`) for test method names to keep expectations explicit and consistent.
- **Test Naming Convention for validators**: Follow the `Should<Outcome>When<Condition>` style used in `LoginUserValidationTests` (e.g., `ShouldHaveErrorWhenNameIsEmpty`, `ShouldNotHaveErrorWhenPasswordIsValid`) to keep validation intent readable.
