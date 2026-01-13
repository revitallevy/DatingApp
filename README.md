# DatingApp

A full-stack dating application demo built with a .NET backend and an Angular frontend. The API uses Entity Framework Core, ASP.NET Identity, JWT authentication, SignalR for real-time messaging, and supports photo uploads (Cloudinary by default). The client is an Angular app that consumes the API and connects to SignalR hubs for presence and messaging.

## Features

- User registration & login (JWT)
- Roles and authorization (Admin, Moderator)
- Real-time presence and messaging via SignalR
- Photo upload & moderation
- Likes, member lists with paging & filtering
- Seeded demo users for quick local setup

## Tech Stack

- Backend: ASP.NET Core (see `API/`)
- Database: SQL Server (containerized via Docker in repo)
- Frontend: Angular 20 (see `client/`)
- Real-time: SignalR

## Prerequisites

- .NET SDK 10 (or compatible runtime visible in the repo)
- Node.js (recommended 18+)
- npm (bundled with Node.js)
- Docker & Docker Compose (optional but recommended for local DB)

## Quick Start

1. Clone the repo:

```bash
git clone https://github.com/<your-username>/DatingApp.git
cd DatingApp
```

2. Option A — Run with Docker (recommended for database):

```bash
docker compose up -d
```

The included `docker-compose.yml` brings up a SQL Server container with credentials matching the project's development connection string.

3. Start the API

```bash
cd API
dotnet restore
dotnet build
dotnet run
```

Notes:
- On startup the API will apply EF Core migrations automatically (`context.Database.MigrateAsync()` in `Program.cs`) and call the user seed routine to create demo users.
- Edit the connection string and secret keys in `API/appsettings.Development.json` before running in other environments.

4. Start the client

```bash
cd client
npm install
npm start
```

The Angular app runs at `http://localhost:4200` by default and is configured to call `http://localhost:5001` (API) in development.

## Database & Migrations

- The API will run pending EF migrations automatically at startup and seed demo data via `Seed.SeedUsers(...)`.
- If you prefer manual migration control, install the EF tools and run:

```bash
cd API
dotnet tool install --global dotnet-ef
dotnet ef database update
```

## Environment & Configuration

- Important config lives in `API/appsettings.Development.json`. Update the following as needed:
  - `ConnectionStrings:DefaultConnection` — database connection
  - `TokenKey` — JWT signing key
  - `CloudinarySettings` — cloudinary account details for photo uploads (if used)

## Docker

- `docker-compose.yml` in the repo contains a SQL Server service exposing port `1433`.
- Bring the DB up with `docker compose up -d`, then run the API as above.

## Project Structure (top-level)

- `API/` — ASP.NET Core backend (controllers, entities, data, services)
- `client/` — Angular frontend
- `docker-compose.yml` — quick local SQL Server setup

## Useful Files

- `API/Program.cs` — application startup and service wiring
- `API/appsettings.Development.json` — development configuration (connection string, token key)
- `client/package.json` — frontend scripts (`npm start`, `npm build`)

## Development Notes

- SignalR hubs are defined in `API/SignalR/` and mapped in `Program.cs`.
- The API includes middleware for global exception handling and a fallback controller to serve the client SPA.
