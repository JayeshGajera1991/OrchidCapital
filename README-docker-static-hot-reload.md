# Docker static assets hot-reload & recent fixes

This document explains recent changes to the compose setup and the repo so you can update JS/CSS/design assets without rebuilding images every time. It also summarizes earlier fixes that were applied during troubleshooting.

## What changed

- docker-compose.yml
  - `orchid-dashboard` now bind-mounts `./OrchidCapital/wwwroot` into the container at `/app/wwwroot` (read-only). This makes any edits to files under `OrchidCapital/wwwroot` on the host visible immediately in the running container.
  - `orchid-api` now bind-mounts `./OrchidCapitalCoreAPI/wwwroot` into the container at `/app/wwwroot` (read-only) for the same reason.
  - The existing DataProtection keys mount (`./docker/dataprotection-keys:/root/.aspnet/DataProtection-Keys`) remains unchanged.

> Note: We use read-only mounts (`:ro`) to avoid accidental writes from the container. Change to `:rw` only if you need the container to generate files there.

## Why this helps

- Static assets (JS, CSS, images) served from `wwwroot` are not embedded in the published binary; overriding `/app/wwwroot` with a host bind-mount lets you change those files and see updates immediately. This avoids a full `docker compose build` for small client-side changes.

## What still requires a rebuild / redeploy

- Any changes to compiled code (C#), views compiled at publish-time, or NuGet/dependency changes still require rebuilding the image and restarting the container.
- Changes to Razor views (`.cshtml`) typically require a new publish/build unless you run the app in a special development mode that supports runtime view compilation.

## How to update static assets (JS/CSS/design)

1. Edit files under the workspace folder:

   - Dashboard static files: `OrchidCapital/wwwroot/...`
   - API static files: `OrchidCapitalCoreAPI/wwwroot/...`

2. In most cases the running container will pick up the changes immediately. If not, restart the service (no rebuild required):

```bash
docker compose restart orchid-dashboard
docker compose restart orchid-api
```

3. Hard-refresh your browser (Ctrl+F5) to avoid cached assets.

## Security and production notes

- Do not use bind-mounts for production images built and deployed via CI. For production, bake assets into the image or into a CDN / static-file service and use secure secrets for connection strings.
- Ensure the DataProtection keys directory remains shared across services and persisted (already mounted at `./docker/dataprotection-keys`). Losing keys will invalidate auth cookies/antiforgery tokens.

## Summary of other fixes applied during the troubleshooting session

- Fixed login encryption contract: the dashboard previously re-encrypted the password before calling the API. The dashboard now sends the plaintext password over HTTPS to the API and the API performs expected encryption/decryption flow internally. See `OrchidCapital/Controllers/LoginController.cs` for the change.
- Shared DataProtection key ring: `./docker/dataprotection-keys` is mounted into both services so antiforgery tokens and authentication cookies are validated across containers.
- Restored host DB into container: backup and restore scripts were added/updated under `docker/` (see `docker/backup-host-db.ps1` and `docker/restore-to-container.ps1`).
- Fixed Linux case-sensitivity for static paths: several views were updated to reference `~/Admin/Js/...` (capitalization matched), and `wwwroot/Admin/js` was created to ensure the files are served on Linux containers.
- Ensured SQL Server container starts with a compliant `SA_PASSWORD` (`Sa@12345`) and persisted DB/backup volumes under `./docker`.

## How to contribute changes and test quickly

1. Edit your JS/CSS under `wwwroot`.
2. Restart only the service if needed (see commands above).
3. Reproduce the admin action in browser and inspect DevTools Network/Console.

If you want, I can add a small `watch` script (Node.js-based) that auto-copies assets into the host `wwwroot` or sets up live-reload tooling. Tell me if you'd like that.

---
Generated on 2026-07-09 by the maintenance automation.
