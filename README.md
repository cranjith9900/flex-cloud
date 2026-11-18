# Flex Cloud

A full-stack sample matching the Flex Cloud project stack: an interior-design-style CRM/project-management app built with **.NET Core 8 + Entity Framework Core + SQL Server** on the backend and **Angular 14 + TypeScript** on the frontend, with KPI dashboard reporting (DevExpress-style) and notes on how the production app wires in Azure Functions, Service Bus, Event Grid, Cosmos DB, Redis and IdentityServer.

## Structure

```
FlexCloud/
  server/FlexCloud.Api/     .NET Core 8 Web API (Contacts, Leads, Projects, Documents, Dashboard)
  flexcloud-client/         Angular 14 app (Dashboard, Project, Document, Contact, Lead modules)
```

## Domain

- **Contact** — a client/stakeholder
- **Lead** — a prospective project (status: New → Qualified → Converted/Lost), linked to a Contact
- **Project** — an interior-design project (status: Planning/InProgress/OnHold/Completed), linked to a Contact
- **Document** — a file record (floor plan, contract, invoice, render image) attached to a Project
- **Dashboard** — KPI tiles + charts: total leads/projects/documents, open pipeline value, active project budget, conversion rate, leads by status, project budget by status, monthly pipeline trend

## Backend — run the API

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download) (not installed in this environment, so it hasn't been compiled here — the code is a complete, standard `dotnet new webapi` + EF Core layout).

```bash
cd server/FlexCloud.Api
dotnet restore
dotnet run
```

By default (`appsettings.json` → `"UseSqlite": true`) it runs against a local SQLite file (`flexcloud.db`) so it works with zero extra setup — on startup it calls `EnsureCreated()` and seeds 2 contacts, 3 leads, 2 projects and 3 documents.

To use **SQL Server** as in the real stack:

1. In `appsettings.json`, set `"UseSqlite": false`.
2. Point `ConnectionStrings:SqlServer` at your instance (default assumes LocalDB).
3. Optionally switch `EnsureCreated()` to real migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

API runs at `http://localhost:5090`, with Swagger UI at `http://localhost:5090/swagger`.

### Endpoints

- `GET/POST /api/contacts`, `GET/PUT/DELETE /api/contacts/{id}`
- `GET/POST /api/leads` (supports `?status=`), `GET/PUT/DELETE /api/leads/{id}`
- `GET/POST /api/projects` (supports `?status=`), `GET/PUT/DELETE /api/projects/{id}`
- `GET/POST /api/documents` (supports `?projectId=`), `DELETE /api/documents/{id}`
- `GET /api/dashboard/kpis`, `/api/dashboard/leads-by-status`, `/api/dashboard/projects-by-status`, `/api/dashboard/monthly-pipeline?months=6`

## Frontend — run the Angular app

```bash
cd flexcloud-client
npm install
npm start
```

Opens at `http://localhost:4200`. It calls the API at `http://localhost:5090/api` (see `src/environments/environment.ts`); CORS is already enabled on the API for `http://localhost:4200`.

Screens:
- **Dashboard** — KPI tiles and Chart.js charts (doughnut/bar/line) covering lead funnel, project budget by status, and monthly pipeline trend
- **Projects** — list + add/edit/delete, shows linked contact and attached document count
- **Documents** — list + add, filterable by project
- **Contacts** — list + add/edit/delete
- **Leads** — list + add/edit/delete, with status badges

## Production architecture notes (Azure)

This sample runs entirely locally (SQLite/LocalDB + in-memory cache + no auth), but mirrors how the real Flex Cloud app is deployed:

- **Azure Functions** — background jobs (e.g. document processing, notifications) triggered off queues/timers instead of inline in the Web API
- **Azure Service Bus** — decouples long-running or cross-module work (e.g. "lead converted" → provision a project) via queues/topics
- **Azure Event Grid** — fan-out for event-driven integrations (e.g. notify other systems when a document is uploaded)
- **Azure Cosmos DB** — used alongside SQL Server for document-shaped/high-throughput data (e.g. activity feeds, audit logs) where a relational schema isn't a good fit
- **Redis** — response/KPI caching in front of the dashboard and lookup endpoints; this sample's `DashboardController` uses `IMemoryCache` as a local stand-in for the same caching pattern
- **IdentityServer** — centralized login/SSO; this sample has no auth gate since it's meant to run standalone

## Notes

- The Angular production build was verified locally (`npx ng build`) and compiles cleanly. A couple of Angular-CLI-14/TypeScript-4.7 compatibility issues (a far-newer transitive `@types/node` pulling in syntax TS 4.7 can't parse) were fixed via a `types: []` + `skipLibCheck` in `tsconfig.json` and an `overrides` pin in `package.json`.
- The .NET API could not be compiled in this environment because the .NET SDK isn't installed here; install it and run `dotnet build` inside `server/FlexCloud.Api` to verify.
