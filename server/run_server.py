import multiprocessing
import sys

import uvicorn


def main():
    if len(sys.argv) != 2:
        raise ValueError("Must provide 'debug' or 'production' as argument")

    mode = sys.argv[1]

    if mode == "production":
        uvicorn.run(
            "main:app",
            host="0.0.0.0",
            port=8000,
            workers=multiprocessing.cpu_count(),
            log_level="warning",
            access_log=True,
        )
    elif mode == "debug":
        uvicorn.run("main:app", reload=True)
    else:
        raise ValueError("Must provide 'debug' or 'production' as argument")


if __name__ == "__main__":
    main()