# NOTE: script never tried with Windows/Powershell
docker compose -f docker/docker-compose.yml up -d
Write-Host "Waiting for SQL Server to start..."
# TODO: retry loop instead of long sleep
Start-Sleep -Seconds 15
Write-Host "Creating database..."
sqlcmd -S localhost,1433 -U sa -P "g@me0fLife" -Q "create database game_of_life"
Write-Host "Running migrations..."
alembic upgrade head