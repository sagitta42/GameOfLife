"""add pattern glider

Revision ID: 1bf804167a7e
Revises: 821629c0a870
Create Date: 2026-03-21 13:47:27.280644

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa

from alembic_migrations import models, utils


# revision identifiers, used by Alembic.
revision: str = '1bf804167a7e'
down_revision: Union[str, Sequence[str], None] = '821629c0a870'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None

pattern = models.Pattern(id=1, name="glider", type="spaceship")
coordinates = [(0, 1), (1, 2), (2, 0), (2, 1), (2, 2)]

def upgrade() -> None:
    """Upgrade schema."""
    utils.add_row("pattern_templates", pattern)
    for xy in coordinates:
        coord = models.Coordinates(id=pattern.id, x=xy[0], y=xy[1])
        utils.add_row("pattern_coordinates", coord)


def downgrade() -> None:
    """Downgrade schema."""
    for table_name in ["pattern_coordinates", "pattern_templates"]:
        utils.delete_row_by_id(table_name, pattern)
