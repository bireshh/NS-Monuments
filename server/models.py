from sqlalchemy import Column, String, DateTime
from datetime import datetime, timezone
from database import Base


class ImageRecord(Base):
    __tablename__ = "image_records"

    uuid = Column(String, primary_key=True, index=True)
    description = Column(String, nullable=False)
    created_at = Column(DateTime, default=lambda: datetime.now(timezone.utc))