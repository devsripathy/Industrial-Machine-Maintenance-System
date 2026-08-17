# 🏭 Industrial Machine Maintenance System

**A Clean Architecture CMMS / Asset Performance Management platform, built on .NET 9.**

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-2ea44f)](#-architecture)
[![Database](https://img.shields.io/badge/DB-MySQL%20%7C%20SQLite-4479A1?logo=mysql&logoColor=white)](#-tech-stack)
[![Auth](https://img.shields.io/badge/Auth-JWT-000000?logo=jsonwebtokens&logoColor=white)](#-api-reference)
[![Status](https://img.shields.io/badge/Status-Backend%20Runnable-yellow)](#-current-status)

> Internal codename: **SentinelOps** — a context-aware Industrial Asset Intelligence platform, designed to move maintenance operations away from static thresholds and fixed schedules toward data-driven decisioning.

---

## 📖 Overview

Traditional CMMS tools schedule maintenance on fixed intervals. Traditional Predictive Maintenance tools trigger on static sensor thresholds. Both approaches miss context — they don't reason about machine history, criticality, or operational load when deciding what to act on.

**Industrial Machine Maintenance System** is being built as the backend foundation for a platform that treats maintenance as an operational decision problem, not a checklist. Today, that foundation is a working, testable, secured API. The AI/context-aware decisioning layer described in the platform vision is the direction the project is headed, not a claim about what's shipped yet — see [Current Status](#-current-status) for exactly what runs today.

---

## 🏗️ Architecture

The backend follows **Clean Architecture**, keeping business rules independent of frameworks, databases, and delivery mechanisms.

```
┌─────────────────────────────────────────────┐
│                Presentation                  │  ← API Controllers, Auth Middleware, DI Composition Root
├─────────────────────────────────────────────┤
│                Infrastructure                 │  ← EF Core, MySQL/SQLite Providers, Repositories
├─────────────────────────────────────────────┤
│                 Application                   │  ← Use Cases, DTOs, Business Workflows
├─────────────────────────────────────────────┤
│                   Domain                       │  ← Entities, Value Objects, Core Business Rules
└─────────────────────────────────────────────┘
```

**Why this matters:** the Domain layer has zero dependency on EF Core, MySQL, or ASP.NET — meaning the persistence layer can swap between SQLite (dev) and MySQL (prod) without touching business logic, and the CQRS/MediatR layer (in progress) can be added without restructuring anything underneath it.

---

## 🧰 Tech Stack

| Layer | Technology |
|---|---|
| **Backend Framework** | .NET 9 / ASP.NET Core |
| **Architecture Pattern** | Clean Architecture (Domain → Application → Infrastructure → Presentation) |
| **Database (Production)** | MySQL |
| **Database (Development)** | SQLite |
| **ORM** | Entity Framework Core |
| **Authentication** | JWT Bearer Tokens |
| **Containerization** | Docker / Docker Compose |
| **CI** | GitHub Actions (`.github/workflows`) |
| **Frontend (Planned)** | React + Tailwind CSS |

---

## ✅ Current Status

This is what is **actually implemented and runnable** today:

- ✅ Backend boots locally and via Docker Compose
- ✅ JWT authentication with seeded admin credentials
- ✅ Protected `machines` and `machineTypes` endpoints
- ✅ Health check endpoint
- ✅ Dual-database setup: SQLite for local dev (no Docker daemon required), MySQL for the production-style stack
- ✅ EF Core migrations in place (`InitialCreate`)

**Not yet built** (see [Roadmap](#-roadmap)): the React frontend, full CQRS/MediatR command layer, work-order lifecycle, PM (preventive maintenance) scheduling, inventory management, QR-code scanning, KPI dashboards, and notifications.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Docker + Docker Compose (optional — only needed for the MySQL stack)

### Option 1 — Run the backend locally (SQLite, no Docker)

```bash
cd backend
$env:ASPNETCORE_ENVIRONMENT='Development'
$env:ASPNETCORE_URLS='http://localhost:5050'
dotnet run --project src/Presentation/SentinelOps.Presentation.csproj
```

> The commands above use PowerShell syntax (`$env:`). On macOS/Linux, use:
> ```bash
> export ASPNETCORE_ENVIRONMENT=Development
> export ASPNETCORE_URLS=http://localhost:5050
> dotnet run --project src/Presentation/SentinelOps.Presentation.csproj
> ```

### Option 2 — Run the full stack with Docker Compose (MySQL)

```bash
docker compose up -d --build
```

This starts:

| Service | Host Port | Notes |
|---|---|---|
| MySQL | `3306` | Production-style persistence |
| Backend API | `5050` | Maps host `5050` → container `8080` |

### Default seeded credentials

| Field | Value |
|---|---|
| Username | `admin` |
| Password | `AdminPassword123!` |

> ⚠️ Seed credentials only. Rotate or remove before any non-local deployment.

---

## 📡 API Reference

| Method | Endpoint | Auth Required | Description |
|---|---|:---:|---|
| `GET` | `/health` | ❌ | Service health check |
| `POST` | `/api/auth/login` | ❌ | Authenticate and receive a JWT |
| `GET` | `/api/machines` | ✅ | List registered machines |
| `GET` | `/api/machineTypes` | ✅ | List machine type taxonomy |

**Example: Login**

```bash
curl -X POST http://localhost:5050/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"AdminPassword123!"}'
```

**Example: Authenticated request**

```bash
curl http://localhost:5050/api/machines \
  -H "Authorization: Bearer <token-from-login-response>"
```

---

## 🗄️ Database Notes

- `appsettings.Development.json` is configured for **SQLite** so the app runs on a bare workstation with no MySQL service required.
- `docker-compose.yml` is configured for **MySQL**, mirroring the intended production stack.
- The initial EF Core migration lives at `backend/src/Presentation/Migrations/InitialCreate.cs`.

---

## 📁 Project Structure

```
Industrial-Machine-Maintenance-System/
├── .github/workflows/     # CI pipelines
├── backend/
│   └── src/
│       ├── Domain/         # Entities & business rules (planned split)
│       ├── Application/    # Use cases, DTOs (planned split)
│       ├── Infrastructure/ # EF Core, repositories (planned split)
│       └── Presentation/   # SentinelOps.Presentation — API entry point
├── frontend/              # React + Tailwind (scaffolding — not yet built out)
├── docker-compose.yml     # MySQL + backend orchestration
├── buildlog.txt
└── README.md
```

---

## 🛣️ Roadmap

1. **Application layer** — complete CQRS + MediatR for auth, machines, PM plans, and work orders
2. **Frontend** — React + Tailwind app: login, dashboard, machines, work orders, PM plans, inventory, audit logs, settings
3. **Core CMMS features** — work-order lifecycle, PM scheduling, inventory tracking, QR-code scanning, KPI dashboards, notifications
4. **Testing** — unit/integration coverage for permission checks, work-order transitions, and PM generation logic

---

## 🤝 Contributing

This project is under active early-stage development. Issues and pull requests are welcome — please open an issue first for significant changes so scope and architecture direction can be discussed before implementation.

---

## 📄 License

No license has been declared for this repository yet. Until a `LICENSE` file is added, default copyright applies and the code is **not** open for reuse or redistribution. If this is meant to be open source, adding an [MIT](https://choosealicense.com/licenses/mit/) or [Apache 2.0](https://choosealicense.com/licenses/apache-2.0/) license is recommended.

---

<p align="center">Built with .NET 9 and Clean Architecture — maintenance intelligence, done properly.</p>
