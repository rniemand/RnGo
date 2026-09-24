# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

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

## Non-obvious behavior

- **Short code generation**: `LinkService` reads `MAX(LinkId)` once in its constructor and keeps an in-memory `_nextLinkId` counter. The short code is `Base36(_nextLinkId)`, computed *before* the insert, and assumes it matches the DB's auto-increment `LinkId`. This only holds with a single app instance and no gaps in the auto-increment sequence.
- **Duplicate URLs** return the existing link's short code instead of creating a new row.
- **API keys**: `ApiKeyService` caches enabled keys from the DB and refreshes every 10 minutes. Matching is case-insensitive. A refresh swaps in a new set rather than mutating the old one, so if a refresh fails, the previously loaded keys stay valid.
- Soft deletes: queries filter on `Deleted = 0`.

## Conventions

- C# code uses 2-space indentation, file-scoped namespaces, and nullable enabled. SQL strings inside `RepoQueries` use tabs.
- `RnGo.Core.csproj` pins `System.Data.SqlClient` and `System.Drawing.Common` directly. This overrides vulnerable versions that `Rn.NetCore.DbCommon`/`MySql.Data` pull in. Keep these pins until those upstream packages are updated.
