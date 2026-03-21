import enum

from alembic import op
from pydantic import BaseModel
import sqlalchemy as sa


class SaColumnType(enum.Enum):
    int = sa.Integer
    str = sa.String

    @classmethod
    def from_type(cls, t: type) -> SaColumnType:
        return cls[t.__name__]


class Column(BaseModel):
    name: str
    description: str
    type: type
    primary_key: bool = False
    nullable: bool = False
    foreign_key: str | None = None

    @property
    def sa_type(self) -> sa.types.TypeEngine:
        return SaColumnType.from_type(self.type).value()

    def get_sa_column(self) -> sa.Column:
        foreign_key_args = []
        if self.foreign_key is not None:
            foreign_key = sa.ForeignKey(
                name=f"fk_{self.foreign_key.replace('.','_')}",
                column=self.foreign_key,
            )
            foreign_key_args = [foreign_key]

        return sa.Column(
            self.name,
            self.sa_type,
            nullable=self.nullable,
            primary_key=self.primary_key,
            *foreign_key_args,
        )


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


def create_table(table: Table):
    op.create_table(
        table.name,
        *(c.get_sa_column() for c in table.columns),
    )
    add_column_descriptions(table)
