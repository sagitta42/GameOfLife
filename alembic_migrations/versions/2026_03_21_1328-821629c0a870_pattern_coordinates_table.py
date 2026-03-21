"""pattern coordinates table

Revision ID: 821629c0a870
Revises: 74d9f1e6e64f
Create Date: 2026-03-21 13:28:31.985124

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa

from alembic_migrations import utils


# revision identifiers, used by Alembic.
revision: str = '821629c0a870'
down_revision: Union[str, Sequence[str], None] = '74d9f1e6e64f'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None

table_coordinates = utils.Table(
    name="pattern_coordinates",
    columns = [
        utils.Column(name="id", description="Pattern ID", type="int", foreign_key="pattern_templates.id"),
        utils.Column(name="x", description="X coordinate of a live cell", type="int"),
        utils.Column(name="y", description="Y coordinate of a live cell", type="int"),
    ]
)

def upgrade() -> None:
    """Upgrade schema."""
    utils.create_table(table_coordinates)


def downgrade() -> None:
    """Downgrade schema."""
    op.drop_table(table_coordinates.name)
