from sqlalchemy.ext.asyncio import create_async_engine, async_sessionmaker, AsyncSession
from sqlalchemy.orm import declarative_base

DATABASE_URL = "sqlite+aiosqlite:///./images.db"

async_engine = create_async_engine(DATABASE_URL, echo=False)
async_session_maker = async_sessionmaker(async_engine, class_=AsyncSession, expire_on_commit=False)
Base = declarative_base()


async def get_db():
    async with async_session_maker() as session:
        yield session