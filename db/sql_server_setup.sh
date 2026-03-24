docker compose -f docker/docker-compose.yml up -d
echo "Waiting for SQL Server to start..."
# TODO: retry loop instead of long sleep
sleep 15
# apt-get install mssql-tools
echo "Creating database..."
sqlcmd -S localhost,1433 -U sa -P "g@me0fLife" -Q "create database game_of_life"
echo "Running migrations..."
alembic upgrade head