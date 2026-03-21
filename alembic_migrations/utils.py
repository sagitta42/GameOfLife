import enum
from typing import Type

from alembic import op
from pydantic import BaseModel
import sqlalchemy as sa

from alembic_migrations.models import Coordinates, DataModel, Pattern


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


def get_model_columns(model: Type[DataModel]) -> list[Column]:
    ret = []
    for name, info in model.model_fields.items():
        col = Column(name=name, description=info.description, type=info.annotation)
        ret.append(col)
    return ret


def make_table_columns(
    model: Type[DataModel],
    primary_keys: list[str] = [],
    foreign_keys: dict[str, str] = {},
) -> list[Column]:
    # TODO: validation (columns in primary/foreign keys do not exist)
    cols = get_model_columns(model)
    for col in cols:
        if col.name in primary_keys:
            col.primary_key = True
        if col.name in foreign_keys:
            col.foreign_key = foreign_keys[col.name]
    return cols


# TODO: seems to be not working
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


def read_table(table_name: str) -> sa.Table:
    metadata = sa.MetaData()
    ret = sa.Table(table_name, metadata, autoload_with=op.get_bind())
    return ret


def add_row(table_name: str, row: DataModel):
    table = read_table(table_name)
    op.execute(table.insert().values(row.model_dump()))


def delete_row_by_id(table_name: str, row: DataModel):
    table = read_table(table_name)
    op.execute(table.delete().where(table.c.id == row.id))


def add_pattern(pattern: Pattern, coordinates: list[tuple[int, int]]):
    add_row("pattern_templates", pattern)
    for xy in coordinates:
        coord = Coordinates(id=pattern.id, x=xy[0], y=xy[1])
        add_row("pattern_coordinates", coord)


def remove_pattern(pattern: Pattern):
    for table_name in ["pattern_coordinates", "pattern_templates"]:
        delete_row_by_id(table_name, pattern)
