from logging.config import fileConfig

from pydantic import BaseModel
from sqlalchemy import URL, engine_from_config
from sqlalchemy import pool

from alembic import context

from alembic_migrations.settings import DBMode, db_settings, get_config

# this is the Alembic Config object, which provides
# access to the values within the .ini file in use.
config = context.config

# Interpret the config file for Python logging.
# This line sets up loggers basically.
if config.config_file_name is not None:
    fileConfig(config.config_file_name)

# add your model's MetaData object here
# for 'autogenerate' support
# from myapp import mymodel
# target_metadata = mymodel.Base.metadata
target_metadata = None

# other values from the config, defined by the needs of env.py,
# can be acquired:
# my_important_option = config.get_main_option("my_important_option")
# ... etc.

class SQLServerConfig(BaseModel):
    username: str
    password: str
    port: int

def get_url() -> URL:

    if db_settings.mode == DBMode.sqlserver.value:
        sqlserver_config = SQLServerConfig(**get_config("sql_server_config.json"))

        url = URL.create(
            "mssql+pyodbc",
            host="localhost",
            port=sqlserver_config.port,
            username=sqlserver_config.username,
            password=sqlserver_config.password,
            database=db_settings.db_name,
            query={"driver": "ODBC Driver 17 for SQL Server"},
        )
    elif db_settings.mode == DBMode.sqlite.value:
        url = URL.create("sqlite", database=f"db/{db_settings.db_name}.db")
    else:
        raise NotImplementedError("DB mode not recognized; use 'sqlserver' or 'sqlite'")
    return url


def run_migrations_offline() -> None:
    """Run migrations in 'offline' mode.

    This configures the context with just a URL
    and not an Engine, though an Engine is acceptable
    here as well.  By skipping the Engine creation
    we don't even need a DBAPI to be available.

    Calls to context.execute() here emit the given string to the
    script output.

    """
    context.configure(
        url=get_url(),
        target_metadata=target_metadata,
        literal_binds=True,
        dialect_opts={"paramstyle": "named"},
    )

    with context.begin_transaction():
        context.run_migrations()


def run_migrations_online() -> None:
    """Run migrations in 'online' mode.

    In this scenario we need to create an Engine
    and associate a connection with the context.

    """
    connectable = engine_from_config(
        config.get_section(config.config_ini_section, {}),
        prefix="sqlalchemy.",
        poolclass=pool.NullPool,
        url=get_url(),
    )

    # TODO: investigate warning:
    # SAWarning: Unrecognized server version info '17.0.1000.7'.  Some SQL Server features may not function properly.
    with connectable.connect() as connection:
        context.configure(
            connection=connection, target_metadata=target_metadata
        )

        with context.begin_transaction():
            context.run_migrations()


if context.is_offline_mode():
    run_migrations_offline()
else:
    run_migrations_online()
