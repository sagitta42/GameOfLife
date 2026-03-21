"""pattern coordinates table

Revision ID: 821629c0a870
Revises: 74d9f1e6e64f
Create Date: 2026-03-21 13:28:31.985124

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa

from alembic_migrations import models, utils


# revision identifiers, used by Alembic.
revision: str = '821629c0a870'
down_revision: Union[str, Sequence[str], None] = '74d9f1e6e64f'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None

columns = utils.make_table_columns(
    models.Coordinates, foreign_keys={"id": "pattern_templates.id"}
)
table_coordinates = utils.Table(name="pattern_coordinates", columns=columns)

def upgrade() -> None:
    """Upgrade schema."""
    utils.create_table(table_coordinates)


def downgrade() -> None:
    """Downgrade schema."""
    op.drop_table(table_coordinates.name)
