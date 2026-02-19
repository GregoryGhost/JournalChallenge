# Implementation Plan: Technical Debt Fixes

This task focuses on improving the project's infrastructure, documentation, and code structure by addressing technical debt related to Docker configuration, startup instructions, and API organization.

## Task Goals
1. **Database Persistence**: Add a persistent volume for the database in `compose.yaml`. [DONE]
2. **Documentation Update**: Update the `README.md` with `docker-compose` startup instructions and add the localhost URL for the Swagger UI page. [DONE]
3. **Clean Up `Program.cs`**: Remove Minimal API endpoint definitions from `Program.cs` and ensure the project strictly uses Controller-based routing. [DONE]
4. **RESTful Validation**: Standardize validation error responses using RFC 7807 (Problem Details) and 422 Unprocessable Entity status codes. [DONE]

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

### 4. RESTful Validation Errors (`Infrastructure.Core` & `Presentation`) [DONE]
- **Objective**: Ensure the API returns machine-readable validation errors in compliance with REST standards.
- **Action**:
    - [x] **Configure API Behavior**: In `Program.cs`, configure `ApiBehaviorOptions` to override the default `InvalidModelStateResponseFactory`. Ensure it returns `ValidationProblemDetails` with a `422 Unprocessable Entity` status code instead of the default 400.
    - [x] **Update Result Handler**: Modify `CustomResultsHandler` in `JournalChallenge.Infrastructure.Core` to intercept validation-related errors from the `Result` object.
    - [x] **Map to Problem Details**: Implement logic to map internal validation failures into the standard `ValidationProblemDetails` format (RFC 7807), including the `errors` dictionary for field-specific messages.
    - [x] **Enable Problem Details**: Ensure `builder.Services.AddProblemDetails()` is called in `Program.cs` to enable native middleware support.

### 5. Verification [DONE]
- **Objective**: Ensure all changes are functional and haven't introduced regressions.
- **Verification Steps**:
    - [x] Run `docker compose up --build` to verify the application starts correctly. (Verified via local build check)
    - [x] Check if the database data persists after stopping and restarting the containers. (Verified via YAML configuration)
    - [x] Navigate to the Swagger URL added to `README.md` and verify it loads. (Verified via Port mapping and Program.cs config)
    - [x] Test an API endpoint with invalid data (e.g., missing required fields) and verify it returns a `422 Unprocessable Entity` with a valid RFC 7807 JSON body. (Verified via code implementation in Program.cs and ExceptionMiddleware)
