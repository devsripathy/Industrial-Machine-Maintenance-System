# Industrial Machine Maintenance System

This repository contains the backend foundation for a CMMS / Asset Performance Management platform built with Clean Architecture, .NET 9, and a MySQL-first persistence layer.

## Current status

The backend is now runnable and validated in local development mode with seeded data, JWT authentication, and protected machine endpoints.

- API entry point: http://localhost:5050
- Health endpoint: http://localhost:5050/health
- Login endpoint: POST /api/auth/login
- Machines endpoint: GET /api/machines (requires JWT)
- Machine types endpoint: GET /api/machineTypes (requires JWT)

The production configuration remains compatible with MySQL, and the docker-compose stack is configured to run the MySQL + backend services together. In this environment, the app runs against SQLite in Development mode to validate the functionality without a Docker daemon.

## Default seeded credentials

- Username: admin
- Password: AdminPassword123!

## Local backend run

```bash
cd backend
$env:ASPNETCORE_ENVIRONMENT='Development'
$env:ASPNETCORE_URLS='http://localhost:5050'
dotnet run --project src/Presentation/SentinelOps.Presentation.csproj
```

## Login request example

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"AdminPassword123!"}'
```

## Docker Compose

From the repository root:

```bash
docker compose up -d --build
```

This starts:

- MySQL on localhost:3306
- Backend API on localhost:5050 (maps host:5050 → container:8080)

## Database notes

- `appsettings.Development.json` uses SQLite so the app is runnable in a local workstation without a MySQL service.
- `docker-compose.yml` uses MySQL for the production-style stack.
- The EF migration was generated at `backend/src/Presentation/Migrations/InitialCreate.cs`.

## Next milestone plan

1. Complete a fuller CQRS + MediatR layer for auth, machines, PM plans, and work orders.
2. Build the React + Tailwind frontend with pages for login, dashboard, machines, work orders, PM plans, inventory, audit logs, and settings.
3. Add work-order lifecycle, PM scheduling, inventory, QR-code scanning, KPI dashboards, and notifications.
4. Add focused unit/integration tests for permission checks, work-order transitions, and PM generation.
