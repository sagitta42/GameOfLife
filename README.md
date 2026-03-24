# Game Of Life

[Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)

Portfolio project demonstrating skills in:
- C#
- .NET Core / .NET 8 / .NET ASP
- .NET ASP MVC / Razor / HTML / CSS / Javascript
- SQLite / SQL Server / DB migrations / alembic / sqlalchemy
- Docker

## SQL Server demo

For SQL Server demo
0. Clone this repository
1. Set up SQL Server (docker) and migrate data: run `db/sql_server_setup.sh` or `db/sql_server_setup.ps1` (warning: ps1 script has not been tested, in case of issues run bash script on Windows)
2. Get `.zip` or `.tar.gz` assets from a [release](https://github.com/sagitta42/GameOfLife/releases) above v2.2.0
3. In assets in `db/db_settings.json` change "mode" from "sqlite" to "sqlserver"
4. Run the executable `./GameOfLife` or `GameOfLife.exe` to launch game

In this mode, the game backend will be connecting to the SQL server to query the database instead of the default light SQLite mode.

Note: `2019-latest` image in `docker-compose.yml` since using `17.0.1000.7` (Driver 17) in alembic migrations <-> SQL Server 2019 

## How to build project

0. Clone this repository

1. Install python packages necessary for database migration: `pip install -r requirements.txt`

2. Migrate: `alembic upgreade head` (if building for SQLite mode)

3. Publish

```bash
dotnet publish WebApp/GameOfLife.csproj --self-contained --configuration Release
```

For different os: `--os win`

### SQLite VS SQL Server mode

Mode under `db/db_settings.json` determines:

1. Where alembic looks to migrate
2. Where C# backend looks to query

SQLite mode:
- necessary to have mode as `"sqlite"` during build - migrated db included in published asset
- `db_settings.json` itself is included in asset - SQLite db will be queried by C# backend when the executable is running

SQL Server
- not necessary to migrate anything during build
- set `"sqlserver"` mode during build to have the correct mode in assets (not necessary but convenient)