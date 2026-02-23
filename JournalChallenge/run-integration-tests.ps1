# 1. Clean up any stale containers or orphaned networks from previous runs
Write-Host "Cleaning up previous environment..." -ForegroundColor Cyan
docker compose -f docker.testing.yaml down --remove-orphans

# 2. Start the infrastructure (Database, Migrator, and API) in detached mode (-d)
# This prevents Docker Compose from shutting down the DB prematurely.
Write-Host "Starting infrastructure services (DB, Migrate, API)..." -ForegroundColor Cyan
docker compose -f docker.testing.yaml up -d --build db migrate api

# 3. Run Newman tests using 'docker compose run'
# This command honors 'depends_on: condition: service_healthy' and waits for the API.
# The '--rm' flag ensures the temporary Newman container is removed after execution.
Write-Host "Executing integration tests via Newman..." -ForegroundColor Green
docker compose -f docker.testing.yaml run --rm newman

# 4. Capture the exit code of the Newman process ($LASTEXITCODE in PowerShell)
$testExitCode = $LASTEXITCODE

# 5. Final cleanup: stop and remove all remaining infrastructure containers
Write-Host "Tests finished. Shutting down environment..." -ForegroundColor Yellow
docker compose -f docker.testing.yaml down

# 6. Exit the script with Newman's return code to notify CI/CD (GitHub Actions, etc.)
if ($testExitCode -ne 0) {
    Write-Host "RESULT: Integration tests FAILED (Exit Code: $testExitCode)" -ForegroundColor Red
    exit $testExitCode
} else {
    Write-Host "RESULT: Integration tests PASSED successfully!" -ForegroundColor Green
    exit 0
}
