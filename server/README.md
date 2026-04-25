# Image Storage API

## Overview
A simple FastAPI server for storing images with metadata in SQLite.

## API Documentation
Interactive docs available at `https://your-domain.com/docs`

## Endpoints

### Upload Image
Upload an image file with an optional description.

**Request:**
```bash
POST /images
Content-Type: multipart/form-data

file: (binary)
description: "My photo"  # optional
```

**Response:**
```json
{
  "message": "Image uploaded successfully"
}
```

---

### List Images
Get all stored images metadata.

**Headers:**
```
admin_api_key: your-secret-api-key
```

**Request:**
```bash
GET /images
```

**Response:**
```json
[
  {
    "uuid": "550e8400-e29b-41d4-a716-446655440000.jpg",
    "description": "My photo",
    "created_at": "2026-04-25T15:30:00Z"
  }
]
```

---

### Download Image
Download an image by its UUID.

**Headers:**
```
admin_api_key: your-secret-api-key
```

**Request:**
```bash
GET /images/{uuid}
```

**Response:**
Binary image file (e.g., `image/jpeg`)

---

### Delete Image
Delete an image and its metadata.

**Headers:**
```
admin_api_key: your-secret-api-key
```

**Request:**
```bash
DELETE /images/{uuid}
```

**Response:**
```json
{
  "message": "Image deleted successfully"
}
```

## Setup

1. Create a `.env` file with `ADMIN_API_KEY=your-secret-key`
2. Install dependencies: `uv sync`
3. Run in debug mode: `uv run python run_server.py debug`
4. Run in production: `uv run python run_server.py production`