# EngaGenie

This repository contains the code and project files for my bachelor thesis.

## Cloning the Repository

Clone the repository using:

```bash
git clone https://github.com/MiraZzle/hlasovatko-bachelor-thesis.git
cd hlasovatko-bachelor-thesis
```

## How to Run

### Option 1: Run with Docker Compose

Make sure Docker and Docker Compose are installed on your machine

1. Create a `.env` file in the project root with the following content:

   ```env
   CLIENT_PORT=3000
   MANAGER_PORT=3001
   BACKEND_PORT=5000

   VITE_BACKEND_URL=http://localhost:5000
   VITE_MANAGER_URL=http://localhost:3001
   VITE_CLIENT_URL=http://localhost:3000

   JWT_SECRET=supersecretkey
   JWT_EXPIRATION=1h
   ```

2. To run with compose, use the following commands:

   ```bash
   docker compose build --no-cache
   docker compose up
   ```

- The student client will be available at `http://localhost:3000`
- The teacher manager interface will be at `http://localhost:3001`
- The backend API (if enabled) will be at `http://localhost:5000`

> Tip: If needed, uncomment the `backend` service in `docker-compose.yml`.

### Option 2: Run manually with pnpm

Make sure [Node.js](https://nodejs.org/) (v18+) and [pnpm](https://pnpm.io/) are installed.

1. Create a `.env` file in each frontend folder (`/client` and `/manager`) with appropriate environment variables:

   Example for `/manager/.env`:

   ```env
   VITE_BACKEND_URL=http://localhost:5000
   VITE_CLIENT_URL=http://localhost:3000
   ```

   Example for `/client/.env`:

   ```env
   VITE_BACKEND_URL=http://localhost:5000
   VITE_MANAGER_URL=http://localhost:3001
   ```

2. Install dependencies:

   ```bash
   pnpm install
   ```

3. Run the desired frontend:

   ```bash
   pnpm run dev
   ```

> If you want to run both frontends at once, open two terminals and run the same command inside `/client` and `/manager`.

## Documentation

For detailed documentation, visit the project [wiki](https://github.com/MiraZzle/hlasovatko-bachelor-thesis/wiki).

## Overleaf

The [thesis](https://www.overleaf.com/read/ghkpfkxdbsyv#629916) is written in LaTeX and managed on Overleaf.
