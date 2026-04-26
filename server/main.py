import os
import uuid as uuid_lib
from contextlib import asynccontextmanager
from datetime import datetime, timezone
from pathlib import Path

import aiofiles
from dotenv import load_dotenv
from fastapi import FastAPI, HTTPException, UploadFile, File, Form, Header, Depends
from fastapi.responses import FileResponse
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from database import async_engine, get_db, Base
from models import ImageRecord
from schemas import ImageRecordResponse

load_dotenv()

IMAGES_DIR = Path("images")
IMAGES_DIR.mkdir(exist_ok=True)


async def require_admin_api_key(admin_api_key: str = Header(...)):
    if admin_api_key != os.environ["ADMIN_API_KEY"]:
        raise HTTPException(status_code=401, detail="Invalid API key")


@asynccontextmanager
async def lifespan(app: FastAPI):
    async with async_engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)
    yield


app = FastAPI(lifespan=lifespan)


@app.post("/images")
async def upload_image(
    file: UploadFile = File(...),
    description: str = Form(""),
    db: AsyncSession = Depends(get_db),
):
    file_uuid = f"{uuid_lib.uuid4()}.{file.filename.split('.')[-1]}"
    file_path = IMAGES_DIR / file_uuid

    async with aiofiles.open(file_path, "wb") as buffer:
        await buffer.write(file.file.read())

    record = ImageRecord(
        uuid=file_uuid,
        description=description,
        created_at=datetime.now(timezone.utc),
    )
    db.add(record)
    await db.commit()

    return {"message": "Image uploaded successfully"}


@app.get("/images", response_model=list[ImageRecordResponse])
async def get_images(db: AsyncSession = Depends(get_db), _=Depends(require_admin_api_key)):
    result = await db.execute(select(ImageRecord))
    records = result.scalars().all()
    return [ImageRecordResponse.model_validate(r) for r in records]


@app.get("/images/{uuid}")
async def download_image(uuid: str, db: AsyncSession = Depends(get_db), _=Depends(require_admin_api_key)):
    result = await db.execute(select(ImageRecord).filter(ImageRecord.uuid == uuid))
    record = result.scalar_one_or_none()
    if not record:
        raise HTTPException(status_code=404, detail="Image not found")

    file_path = IMAGES_DIR / uuid
    if not file_path.exists():
        raise HTTPException(status_code=404, detail="Image file not found")

    return FileResponse(file_path)


@app.delete("/images/{uuid}")
async def delete_image(uuid: str, db: AsyncSession = Depends(get_db), _=Depends(require_admin_api_key)):
    result = await db.execute(select(ImageRecord).filter(ImageRecord.uuid == uuid))
    record = result.scalar_one_or_none()
    if not record:
        raise HTTPException(status_code=404, detail="Image not found")

    file_path = IMAGES_DIR / uuid
    if file_path.exists():
        file_path.unlink()

    await db.delete(record)
    await db.commit()

    return {"message": "Image deleted successfully"}