# Game Of Life

[Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)

## SQL Server

For SQL Server demo
0. Clone this repository
1. Set up SQL Server (docker) and migrate data: run `db/sql_server_setup.sh` or `db/sql_server_setup.ps1` (warning: ps1 script has not been tested, in case of issues run bash script on Windows)
2. In assets in `db/db_settings.json` change "mode" from "sqlite" to "sqlserver".

Note: `2019-latest` image in `docker-compose.yml` since using `17.0.1000.7` (Driver 17) in alembic migrations <-> SQL Server 2019 