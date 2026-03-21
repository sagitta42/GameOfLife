from pydantic import BaseModel, Field

class DataModel(BaseModel):
    pass

class Pattern(DataModel):
    id: int = Field(description="ID")
    name: str = Field(description="Pattern name")
    type: str = Field(description="Pattern type")


class Coordinates(DataModel):
    id: int = Field(description="Pattern ID")
    x: int = Field(description="X coordinate of a live cell")
    y: int = Field(description="Y coordinate of a live cell")
