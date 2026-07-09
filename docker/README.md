Steps to move host `OrchidCapital` database into Docker SQL Server and run the app fully in Docker

1. From project root, create the backup from the host SQL Server:

```powershell
# creates docker/mssql-backup/OrchidCapital.bak
powershell -ExecutionPolicy Bypass -File .\docker\backup-host-db.ps1
```

2. Start the Docker SQL Server (and other services):

```powershell
# starts all services (api, dashboard and mssql container)
docker compose up -d
```

3. Restore the backup inside the container (this script will show logical file names first):

```powershell
powershell -ExecutionPolicy Bypass -File .\docker\restore-to-container.ps1
```

4. If the automated restore fails, inspect the FILELISTONLY output and run a manual RESTORE mapping the logical filenames to `/var/opt/mssql/data/*.mdf` and `.ldf`.

5. Validate the apps are connected to the containerized DB:

- Dashboard: http://localhost:5000
- API / Swagger: http://localhost:5001/swagger/index.html

Notes:
- `docker-compose.yml` has been updated to add `mssql-db` service and to point API/dashboard connection strings to `mssql-db`.
- The scripts assume SA password `sa@123`. Change the password in `docker-compose.yml` and scripts if you want a different password.
