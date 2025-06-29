<img src="./docs/images/engagenie-logo.png" height="70">

![Version Badge](https://img.shields.io/badge/version-1.0.0-blue?style=flat)
![Contributors Badge](https://img.shields.io/badge/contributors-1-green?style=flat)
![License Badge](https://img.shields.io/badge/license-MIT-red?style=flat)

## About

A real-time classroom engagement platform for improving and enriching the interaction between teachers and students. The frontend is built using [Svelte](https://svelte.dev/) and [TypeScript](https://www.typescriptlang.org/). The backend is powered by [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) and [C#](https://learn.microsoft.com/en-us/dotnet/csharp/). The platform uses a [PostgreSQL](https://www.postgresql.org/) database, which can be managed with [Adminer](https://www.adminer.org/).

## Running Locally

To run the project locally, you will need [Docker](https://www.docker.com/products/docker-desktop/) and [Docker Compose](https://docs.docker.com/compose/).

First, clone the repository:

```bash
git clone https://github.com/MiraZzle/hlasovatko-bachelor-thesis.git
cd hlasovatko-bachelor-thesis
```

Next, create a `.env` file in the root of the project with the following content (or just rename the file `.env.example` to `.env`):

```env
CLIENT_PORT=3000
MANAGER_PORT=3001
BACKEND_PORT=5000
DB_PORT=5432
ADMINER_PORT=8085

VITE_BACKEND_URL=http://localhost/server
VITE_MANAGER_URL=http://localhost/
VITE_CLIENT_URL=http://localhost/engage

#At least 16 characters
JWT_SECRET=ABCDEFGHIJKLMNOPQRSTUVWXYZ
JWT_EXPIRATION=1h
JWT_ISSUER=http://localhost/server

DB_USER=postgres
DB_PASSWORD=1234
DB_NAME=engagenie

# Initial user login credentials - used for demonstration purposes
A_USER_EMAIL=admin@example.com
A_USER_PASSWORD=admin123456
A_USER_NAME="Admin Veliky"
```

Finally, run the following commands to build and start the application:

```bash
docker compose build --no-cache
docker compose up
```

The applications will be available at the following URLs:

- **Manager (teacher):** [http://localhost/](https://www.google.com/search?q=http://localhost/)
  > Demo: You can login with the A_USER_EMAIL and A_USER_PASSWORD credentials.
- **Client (student):** [http://localhost/engage](https://www.google.com/search?q=http://localhost/engage)
- **Backend API:** [http://localhost/server](https://www.google.com/search?q=http://localhost/server)
- **Adminer (DB UI):** [http://localhost:8085](https://www.google.com/search?q=http://localhost:8085)

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[MIT](/LICENSE.md)

Made by [MiraZzle](https://github.com/MiraZzle)
