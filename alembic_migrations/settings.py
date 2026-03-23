import enum
import json
from pathlib import Path

from pydantic import BaseModel


class DBMode(str, enum.Enum):
    sqlserver = "sqlserver"
    sqlite = "sqlite"


class DBSettings(BaseModel):
    mode: DBMode
    db_name: str

def get_config(config_file: str) -> dict:
    config_path = Path(__file__).parent.parent / config_file
    with open(config_path) as f:
        config = json.load(f)
    return config

db_settings = DBSettings(**get_config("db_settings.json"))