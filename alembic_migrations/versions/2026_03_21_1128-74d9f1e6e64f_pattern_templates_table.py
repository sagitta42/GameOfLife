"""pattern templates table

Revision ID: 74d9f1e6e64f
Revises: 
Create Date: 2026-03-21 11:28:41.037080

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa

from alembic_migrations import utils


# revision identifiers, used by Alembic.
revision: str = '74d9f1e6e64f'
down_revision: Union[str, Sequence[str], None] = None
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None

table_patterns = utils.Table(
    name="pattern_templates",
    columns=[
        utils.Column(name="id", description="ID", type="int", primary_key=True),
        utils.Column(name="name", description="Pattern name", type="str"),
        utils.Column(name="type", description="Pattern type", type="str"),
    ]
)


def upgrade() -> None:
    """Upgrade schema."""
    utils.create_table(table_patterns)

def downgrade() -> None:
    """Downgrade schema."""
    op.drop_table(table_patterns.name)
