# Implementation Plan: Crucial Integration Tests

This task aims to implement a comprehensive set of integration tests to verify the core business rules of the JournalChallenge API, focusing on tree constraints and exception journaling.

## Task Goals
- [ ] Establish a suite of automated integration tests for Tree and Node management.
- [ ] Verify `SecureException` behavior for all defined business rule violations.
- [ ] Implement end-to-end verification for the Exception Journaling system.
- [ ] Ensure API response formats strictly match the specification.

## Testing Infrastructure
The integration tests will use a "black-box" approach powered by **Postman/Newman** and **Docker Compose**.

- **Orchestration**: A dedicated `docker.testing.yaml` will manage the API, PostgreSQL, and a separate **migration runner** container.
- **EF Migration Bundle**: The application Dockerfile will generate a self-contained EF migration bundle.
- **Same Image, Different Entrypoint**: The `migrate` service in Docker Compose will use the API image but override its entrypoint to execute the migration bundle before the API starts.
- **Health Checks**: The application MUST expose a `/health` endpoint. The Dockerfile MUST include `curl` to support native health checks. Docker native health checks will use this endpoint to ensure the database and API are fully ready.
- **Image Pinning**: All Docker images (PostgreSQL, Newman, .NET) MUST be pinned to specific, immutable versions (e.g., `18.2-alpine3.23`) to ensure environment reproducibility.
- **Test Runner**: The official `postman/newman` Docker image will execute the collection.
- **Automation**: The `--exit-code-from` flag will be used to capture test results and automate cleanup.

## Subtasks
Detailed planning and tracking are managed in the [subtasks index](./subtasks/index.md).

- [x] **stt-000**: [Health Check & Infrastructure Setup](./subtasks/stt-000.md) [DONE]
- [x] **stt-005**: [EF Bundle & Migration Runner Setup](./subtasks/stt-005.md) [DONE]
- [x] **stt-001**: [Tree & Node Lifecycle Verification](./subtasks/stt-001.md) [DONE]
- [x] **stt-002**: [Business Constraint Enforcement (SecureException)](./subtasks/stt-002.md) [DONE]
- [x] **stt-003**: [Recursive & Bulk Operations](./subtasks/stt-003.md) [DONE]
- [x] **stt-004**: [Exception Journaling & Audit End-to-End](./subtasks/stt-004.md) [DONE]

## Verification
- [ ] **Execute Tests**: Run the full suite using the following command:
  ```bash
  docker compose -f docker.testing.yaml up --build --exit-code-from newman
  ```
- [ ] **Confirm Exit Code**: Ensure the command returns `0` on success.
- [ ] **Inspect Results**: Review the CLI output and the generated JUnit XML report in the `./tests` directory.
- [ ] **Manual Audit**: Verify that no "Internal Server Error" details are exposed in non-Secure exceptions.
- [ ] **Schema Validation**: Validate JSON schemas for all response types (Tree, Journal Entry, Error).
