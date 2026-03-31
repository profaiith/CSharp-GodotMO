# CSharp-GodotMO Base Template

This repository now contains a **study-friendly, scalable starter architecture** for:

- **Backend:** C# (.NET 8 Web API) with modular MMO feature boundaries.
- **Client:** Godot 4.6-ready C# structure with a 3D FPS character controller.
- **Shared contracts:** DTOs used by both backend and client.

The code is intentionally heavily commented so you can learn the "why" behind each pattern.

---

## 1) Architecture Overview

```text
+-----------------------+             +-------------------------+
|   Godot 4.6 Client    |  HTTP/JSON  |   Backend API Gateway   |
|  (C# + FPS control)   +------------>+  (Feature Modules)      |
|                       |             |                         |
| - Input + camera      |             | - Sessions feature      |
| - Client features     |             | - World simulation      |
| - Prediction hooks    |             | - Repository boundary   |
+-----------+-----------+             +------------+------------+
            |                                        |
            | shared DTO contracts                    |
            v                                        v
      +----------------------+               +----------------------+
      |  GodotMo.Shared      |               | Player persistence   |
      | contracts only       |               | (in-memory now,      |
      +----------------------+               | Redis/DB later)      |
                                             +----------------------+
```

### Why this scales

1. **Feature modules (`IMmoFeature`)** isolate gameplay domains (sessions, world, inventory, guilds, etc.).
2. **Repository interface (`IPlayerRepository`)** allows in-memory today and distributed storage tomorrow.
3. **Shared contracts project** keeps backend/client protocol aligned and versionable.
4. **Client feature registry** mirrors backend modularity for clean growth.

---

## 2) Project Layout

```text
backend/
  src/GodotMo.Backend/
    Abstractions/
    Features/
      Sessions/
      World/
    Infrastructure/
    Program.cs

client/
  GameClient.csproj
  scripts/
    Bootstrap/
    Features/
    Networking/
    Player/

shared/
  src/GodotMo.Shared/
    Contracts/
```

---

## 3) Run Backend

From repository root:

```bash
dotnet run --project backend/src/GodotMo.Backend/GodotMo.Backend.csproj
```

Then open Swagger:

- `http://localhost:5000/swagger` (or the port printed by ASP.NET)

Health endpoint:

- `GET /health`

---

## 4) Godot Client Setup (4.6 + C#)

1. Create/open a Godot 4.6 C# project in the `client/` folder.
2. Ensure .NET support is enabled in your Godot install.
3. Add input actions in Project Settings → Input Map:
   - `move_left` (A)
   - `move_right` (D)
   - `move_forward` (W)
   - `move_backward` (S)
4. Build scene with:
   - `CharacterBody3D` + `FpsCharacterController3D.cs`
   - child `Node3D` as camera pivot
   - child `Camera3D`
5. Point `BackendBaseUrl` to your backend host.

---

## 5) How to Add New Features

### Backend (example: Inventory)

1. Add `Features/Inventory/InventoryFeature.cs` implementing `IMmoFeature`.
2. Register services and endpoints inside the feature.
3. Add the feature to `features` array in `Program.cs`.

### Client

1. Create a class implementing `IClientFeature`.
2. Register it in `FeatureRegistry` / `GameBootstrap`.
3. Add UI, input, and networking hooks isolated in that feature.

---

## 6) Next Steps for Production MMO

- Replace HTTP polling with **WebSocket/ENet** realtime streams.
- Use **server tick loop** and command queues instead of per-request simulation.
- Add **auth provider** (OAuth/JWT) and secure token validation.
- Move player/world state to **Redis + persistent DB**.
- Introduce **world shard services** and cross-shard routing.
- Add **observability** (OpenTelemetry, structured logs, metrics).
- Add **integration tests** for endpoint and simulation correctness.

This template gives a clean baseline so adding these upgrades stays manageable.
