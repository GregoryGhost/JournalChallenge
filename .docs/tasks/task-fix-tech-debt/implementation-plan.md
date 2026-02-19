# Implementation Plan: Technical Debt Fixes

This task focuses on improving the project's infrastructure, documentation, and code structure by addressing technical debt related to Docker configuration, startup instructions, and API organization.

## Task Goals
1. **Database Persistence**: Add a persistent volume for the database in `compose.yaml`.
2. **Documentation Update**: Update the `README.md` with `docker-compose` startup instructions and add the localhost URL for the Swagger UI page.
3. **Clean Up `Program.cs`**: Remove Minimal API endpoint definitions from `Program.cs` and ensure the project strictly uses Controller-based routing.

## Subtasks

### 1. Database Volume (`compose.yaml`) [DONE]
- **Objective**: Ensure database data persists between container restarts.
- **Action**: 
    - [x] Identify the database service in `JournalChallenge/compose.yaml`.
    - [x] Add a `volumes` section to map a named volume or a local directory to the database data directory inside the container.
    - [x] Define the named volume in the top-level `volumes` section if using a named volume.

### 2. Documentation & Swagger (`README.md`) [DONE]
- **Objective**: Improve the developer onboarding experience.
- **Action**:
    - [x] Update `README.md` with clear instructions on how to start the application using `docker compose up`.
    - [x] Explicitly list the localhost URL (e.g., `http://localhost:5000/swagger/index.html`) where the Swagger UI can be accessed after the containers are running.

### 3. API Refactoring (`Program.cs`) [DONE]
- **Objective**: Standardize API routing and clean up `Program.cs`.
- **Action**:
    - [x] Locate any Minimal API endpoint definitions (e.g., `app.MapGet(...)`, `app.MapPost(...)`) in `JournalChallenge.Presentation/Program.cs`.
    - [x] Remove these definitions.
    - [x] Ensure `app.MapControllers()` is present and correctly configured.
    - [x] Verify that all necessary routes are handled by Controllers in `JournalChallenge.Presentation/Controllers/`.

### 4. Verification [DONE]
- **Objective**: Ensure all changes are functional and haven't introduced regressions.
- **Verification Steps**:
    - [x] Run `docker compose up --build` to verify the application starts correctly.
    - [x] Check if the database data persists after stopping and restarting the containers.
    - [x] Navigate to the Swagger URL added to `README.md` and verify it loads.
    - [x] Test an API endpoint to ensure Controller-based routing is working as expected.
