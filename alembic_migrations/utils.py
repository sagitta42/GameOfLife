import enum

from alembic import op
from pydantic import BaseModel
import sqlalchemy as sa

class ColumnType(str, enum.Enum):
    int = "int"
    str = "str"

class SaColumnType(enum.Enum):
    int = sa.Integer
    str = sa.String

class Column(BaseModel):
    name: str
    description: str
    type: ColumnType
    primary_key: bool = False
    nullable: bool = False

    @property
    def sa_type(self) -> sa.types.TypeEngine:
        return SaColumnType[self.type].value()

class Table(BaseModel):
    name: str
    columns: list[Column]

def add_column_descriptions(table: Table):
    for column in table.columns:
        op.execute(f"""
        EXEC sp_addextendedproperty 
            @name = N'MS_Description',
            @value = N'{column.description}',
            @level0type = N'SCHEMA', @level0name = 'dbo',
            @level1type = N'TABLE',  @level1name = '{table.name}',
            @level2type = N'COLUMN', @level2name = '{column.name}';
        """)
