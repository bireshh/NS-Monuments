from pydantic import BaseModel
from datetime import datetime


class ImageRecordResponse(BaseModel):
    uuid: str
    description: str
    created_at: datetime

    class Config:
        from_attributes = True