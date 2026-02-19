# Plan for Implementing Missing Features

This plan outlines the steps required to fulfill the `spec.md` requirements based on the current state of the `JournalChallenge` project.

## Phase 1: Infrastructure & Database Setup
1.  **EF Core Tooling**: Install `Microsoft.EntityFrameworkCore.Design` in the `JournalChallenge.Presentation` project to enable migration commands.
2.  **Initial Migration**: Generate the first EF Core migration (`Initial`) to create `Trees`, `Nodes`, and `ExceptionJournal` tables.
3.  **Database Application**: Apply the migration to the PostgreSQL database (ensuring the `db` service in `compose.yaml` is running).
4.  **Dependency Injection**: Verify and complete the registration of `AppDbContext` and its interface `IApplicationDbContext` across all layers.

## Phase 2: Global Exception Journaling (Core Requirement)
1.  **Global Exception Middleware**: Implement a custom middleware in the Presentation layer to intercept all unhandled exceptions.
    *   **Event ID Generation**: Generate a unique `long` ID (e.g., using `DateTime.UtcNow.Ticks` or a similar unique provider).
    *   **Request Capture**: Implement logic to read and buffer the request body and query parameters for logging.
    *   **Persistence**: Save the exception details (Type, Message, StackTrace, Params, EventID, Timestamp) to the `ExceptionJournal` table.
2.  **Custom Response Formatting**: 
    *   Format `SecureException` responses as: `{"type": "Secure", "id": "...", "data": {"message": "..."}}`.
    *   Format all other exception responses as: `{"type": "Exception", "id": "...", "data": {"message": "Internal server error ID = ..."}}`.

## Phase 3: Application Logic (Trees & Nodes)
1.  **Commands & Queries (CQRS)**:
    *   **`GetTreeQuery`**: Retrieve the full hierarchy of a tree by its `treeName`. Implement auto-creation if the tree name is not found.
    *   **`CreateNodeCommand`**: Logic to add a new node to a tree. Must validate `parentNodeId` (if provided) and enforce unique sibling names within the same tree.
    *   **`RenameNodeCommand`**: Update a node's name, maintaining the unique sibling name constraint.
    *   **`DeleteNodeCommand`**: Implement a "cascade delete" logic to remove a node and all its descendants.
2.  **Validation**: Use FluentValidation to enforce mandatory fields (e.g., node name) and tree-specific constraints.

## Phase 4: Application Logic (Journal)
1.  **`GetJournalRangeQuery`**: Implement pagination (`skip`, `take`) and filtering (Date range `from`/`to`, and a keyword search for the exception message/stack trace).
2.  **`GetJournalSingleQuery`**: Fetch a specific journal entry by its `Id` (primary key).

## Phase 5: Presentation Layer (API Controllers)
1.  **Controller Implementation**: Create or update controllers to match the exact Swagger endpoints:
    *   `POST /api.user.tree.get`
    *   `POST /api.user.tree.node.create`
    *   `POST /api.user.tree.node.rename`
    *   `POST /api.user.tree.node.delete`
    *   `POST /api.user.journal.getRange`
    *   `POST /api.user.journal.getSingle`
    *   `POST /api.user.partner.rememberMe` (Optional Auth stub)
2.  **Route Mapping**: Ensure all routes follow the specified naming convention (e.g., `api.user.tree.get` instead of the standard RESTful `/api/tree`).

## Phase 6: Verification & Testing
1.  **Integration Testing**: Use `JournalChallenge.Tests.Core` to create automated tests verifying:
    *   Tree hierarchy integrity.
    *   Sibling name uniqueness.
    *   Recursive node deletion.
    *   Exception capturing in the journal.
2.  **Manual Testing**: Update and run the `.http` file tests to ensure all endpoints return the expected status codes and JSON payloads.
