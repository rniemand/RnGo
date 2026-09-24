# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

RnGo is a URL shortening and following API: ASP.NET Core (.NET 10) Web API backed by MariaDB/MySQL. Clients POST a URL plus an API key and get a short code back. `GET /f/{shortCode}` redirects to the stored URL and increments its follow count.

## Commands

The solution file lives in `src/`, and the test projects live outside it in `tests/`.

```bash
dotnet build src/RnGo.sln
dotnet test src/RnGo.sln
dotnet test tests/RnGo.Core.T1.Tests --filter "FullyQualifiedName~AddLinkAsyncTests"
dotnet run --project src/RnGo          # http://localhost:5265/swagger
docker compose -f docker/docker-compose.yaml up -d   # local MariaDB, seeded from docker/init/*.sql
docker build . -t rngo                 # run from the repo root (Dockerfile paths are root-relative)
```

The local DB container exposes MariaDB on host port **3308**, but the connection string in `src/RnGo/appsettings.json` uses the default port. Add `Port=3308` when you run against the compose DB.

The Docker image is a **self-contained, fully trimmed** build (`linux-musl-x64`) on `runtime-deps:10.0-alpine`, with invariant globalization and no shared framework. The trimmer removes code that is only reached through reflection, and the build still succeeds when that happens. The failure only shows up at runtime. `src/RnGo/RnGo.csproj` therefore has `TrimmerRootAssembly` entries and feature switches that apply only when `PublishTrimmed=true`. When you add a reflection-heavy dependency (serializers, ORMs, config binding), you may need to add it there. After any change to dependencies or the Dockerfile, build the image and run the end-to-end smoke test (it needs Docker; it starts a seeded MariaDB and exercises every endpoint):

```bash
docker build . -t rngo:local
.github/scripts/smoke-test.sh rngo:local
```

CI (`.github/workflows/docker.yml`) builds and tests the solution first. It then builds the image and runs the smoke test against it. Pull requests stop there. A push to `main` also pushes the image to Docker Hub as `niemandr/rn-go` (`latest` + `sha-<short>` tags), using the `DOCKER_USERNAME`/`DOCKER_PASSWORD` secrets. The Docker build context is the repo root, so `.dockerignore` lives there.

## Architecture

- **`src/RnGo`**: thin web host. `Program.cs` wires up NLog, Swagger, and three DI extensions: `AddRnMetricsBase` and `AddRnDbMySql` (from the external `Rn.NetCore.*` NuGet packages) and `AddRnGo` (this repo). Controllers only delegate to services.
- **`src/RnGo.Core`**: all logic. Layers are Service → Repo → RepoQueries. Everything is registered as a **singleton** in `Extensions/ServiceCollectionExtensions.cs`.
  - `RepoQueries/*` return raw SQL strings (MySQL backtick syntax, `@Param` placeholders). `Repos/*` inherit `BaseRepo<T>` from `Rn.NetCore.DbCommon` and call `ExecuteAsync`/`GetSingle`/etc. with the query and an anonymous parameter object (Dapper-style). To add a DB operation, add a method to the queries interface and class, then to the repo interface and class.
  - Entities in `Entities/` map directly to table columns. The schema is in `docker/init/01-CreateTables.Core.sql` (a copy is in `setup/db.01.tables.sql`).
- **`src/DevConsole`**: scratch console app for calling services by hand against a real DB. It is not part of the product.
- **`tests/RnGo.T1.Tests`**: controller unit tests for the web project.
- **`tests/RnGo.Core.T1.Tests`**: NUnit + NSubstitute unit tests for `RnGo.Core`. Repo tests build a substituted `IBaseRepoHelper` with `TestSupport/BaseRepoHelperFactory`. Tests are grouped per method (e.g. `Services/LinkServiceTests/AddLinkAsyncTests.cs`). SUTs are built through a per-class `TestHelper` that defaults every dependency to a substitute, and test data comes from builders in `TestSupport/Builders`. Test names follow `Method_GivenX_ShouldY` with `// arrange / act / assert` sections.

### Non-obvious behavior

- **Short code generation**: `LinkService` reads `MAX(LinkId)` once in its constructor and keeps an in-memory `_nextLinkId` counter. The short code is `Base36(_nextLinkId)`, computed *before* the insert, and assumes it matches the DB's auto-increment `LinkId`. This only holds with a single app instance and no gaps in the auto-increment sequence.
- **Duplicate URLs** return the existing link's short code instead of creating a new row.
- **API keys**: `ApiKeyService` caches enabled keys from the DB and refreshes every 10 minutes. It uppercases the incoming key before comparing, so keys must be stored in uppercase.
- Soft deletes: queries filter on `Deleted = 0`.
- `LinkService` depends on the `ILoggerAdapter<T>` wrapper from `Rn.NetCore.Common` for testability, while `ApiKeyService` uses `ILogger<T>` directly.

## Conventions

- C# code uses 2-space indentation, file-scoped namespaces, and nullable enabled. SQL strings inside `RepoQueries` use tabs.
- SonarLint rules are in `src/.sonarlint/`.
- `RnGo.Core.csproj` pins `System.Data.SqlClient` and `System.Drawing.Common` directly. This overrides vulnerable versions that `Rn.NetCore.DbCommon`/`MySql.Data` pull in. Keep these pins until those upstream packages are updated.
