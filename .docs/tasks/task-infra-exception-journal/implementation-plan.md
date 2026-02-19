# Implementation Plan: Infrastructure Exception Journal

This task focuses on implementing the global exception journaling system, mapping domain errors to specific exceptions, and providing the API to retrieve journal entries.

## 1. Research & Scaffolding
- **Audit `ExceptionJournal` Entity**: Ensure the `Domain` entity and `Infrastructure` configuration are complete (using `long` IDs, JSON columns for params).
- **Custom Result Handler**: Implement `CustomResultsHandler` in `Infrastructure.Core` to map `DomainError` to `SecureException` or generic `Exception`.
- **Request Context**: Identify the best approach to capture and buffer the request body in ASP.NET Core 8 for logging purposes.

## 2. Global Exception Middleware (Presentation Layer)
- **Middleware Implementation**:
    - Intercept all unhandled exceptions.
    - Generate a unique **Event ID** (using `DateTime.UtcNow.Ticks` or similar).
    - Capture **Query String** and **Request Body** (buffer if necessary).
    - Capture **Stack Trace** and **Exception Type**.
    - Persist all details into the `ExceptionJournal` table using a scoped `DbContext`.
- **Response Formatting**:
    - If `SecureException`: Return HTTP 500 with `{"type": "Secure", "id": "EVENT_ID", "data": {"message": "MSG"}}`.
    - Else: Return HTTP 500 with `{"type": "Exception", "id": "EVENT_ID", "data": {"message": "Internal server error ID = EVENT_ID"}}`.

## 3. Application Logic: Journal Retrieval
Following the **Feature-per-Folder** convention.

### 3.1 Get Journal Range (`GetJournalRangeQuery`)
- **Objective**: Paginated retrieval of journal entries with filtering.
- **DTOs**: `JournalEntryDto` (summarized), `JournalRangeResponse`.
- **Logic**: Apply `skip`, `take`, and filters (`from`, `to`, `search` text).
- **Validation**: Ensure `take` is reasonable, `skip >= 0`.

### 3.2 Get Single Journal Entry (`GetJournalSingleQuery`)
- **Objective**: Detailed view of a specific event.
- **DTOs**: `JournalDetailDto` (includes stack trace and params).
- **Logic**: Fetch by `EventId` or `Id`.
- **Validation**: Ensure ID is provided.

## 4. Unit Testing
Mirroring the Application structure in the Test project.

### 4.1 `Journal/GetRange/`
- **`GetJournalRangeQueryHandlerTests`**: Test pagination and date filtering.
- **`GetJournalRangeQueryValidationTests`**: Test skip/take boundaries.

### 4.2 `Journal/GetSingle/`
- **`GetJournalSingleQueryHandlerTests`**: Test successful retrieval and Not Found cases.
- **`GetJournalSingleQueryValidationTests`**: Test ID validation.

## 5. Verification
- **Build**: Ensure all projects compile.
- **Integration Test**: Throw a dummy exception from a temporary endpoint to verify:
    1.  The entry is saved in the database.
    2.  The response JSON format matches the specification.
- **Documentation**: Final audit and plan update.
