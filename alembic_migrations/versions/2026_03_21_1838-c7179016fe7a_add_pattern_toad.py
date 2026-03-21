"""add pattern toad

Revision ID: c7179016fe7a
Revises: 1bf804167a7e
Create Date: 2026-03-21 18:38:44.569296

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa

from alembic_migrations import models, utils


# revision identifiers, used by Alembic.
revision: str = 'c7179016fe7a'
down_revision: Union[str, Sequence[str], None] = '1bf804167a7e'
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None

pattern = models.Pattern(id=2, name="toad", type="stillife")
coordinates = [(0,1), (0,2), (1,3), (2,0), (3,1), (3,2)]

def upgrade() -> None:
    """Upgrade schema."""
    utils.add_pattern(pattern, coordinates)


def downgrade() -> None:
    """Downgrade schema."""
    utils.remove_pattern(pattern)
