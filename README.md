# EngaGenie

This repository contains the code and project files for my bachelor thesis — a real-time classroom engagement platform.

---

## Cloning the Repository

```bash
git clone https://github.com/MiraZzle/hlasovatko-bachelor-thesis.git
cd hlasovatko-bachelor-thesis
```

---

## How to Run

Requires [Docker](https://www.docker.com/products/docker-desktop/) and [Docker Compose](https://docs.docker.com/compose/).

### 1. Create `.env` file in the root

```env
CLIENT_PORT=3000
MANAGER_PORT=3001
BACKEND_PORT=5000
DB_PORT=5432
ADMINER_PORT=8085

VITE_BACKEND_URL=http://localhost/api
VITE_MANAGER_URL=http://localhost/
VITE_CLIENT_URL=http://localhost/engage

#At least 16 characters
JWT_SECRET=ABCDEFGHIJKLMNOPQRSTUVWXYZ
JWT_EXPIRATION=1h

DB_USER=postgres
DB_PASSWORD=1234
DB_NAME=engagenie
```

### 2. Run

```bash
docker compose build --no-cache
docker compose up
```

- Manager (teacher): [http://localhost/](http://localhost/)
- Client (student): [http://localhost/engage](http://localhost/engage)
- Backend API: [http://localhost/api](http://localhost/api)
- Adminer (DB UI): [http://localhost:8085](http://localhost:8085)

> All traffic goes through NGINX reverse proxy on port **80**.
> Login credentials for testing:
> Email: test@example.com
> Password: 1234

## Password: 1234

## PostgreSQL Management

You can inspect the database with Adminer:

- Go to: [http://localhost:8085](http://localhost:8085)
- Login:

  - System: PostgreSQL
  - Server: `db`
  - User: `postgres`
  - Password: `1234`
  - Database: `engagenie`

---

## Documentation

See the full project wiki: [GitHub Wiki](https://github.com/MiraZzle/hlasovatko-bachelor-thesis/wiki)

---

## Thesis

The thesis is written in LaTeX and available on Overleaf:
[Overleaf Project](https://www.overleaf.com/read/ghkpfkxdbsyv#629916)
