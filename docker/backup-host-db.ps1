# Backup host SQL Server database to docker/mssql-backup/OrchidCapital.bak
# Usage: Run from project root in PowerShell with admin privileges if needed

$scriptFolder = Split-Path -Parent $MyInvocation.MyCommand.Definition
$backupDir = Join-Path $scriptFolder 'mssql-backup'
if (!(Test-Path $backupDir)) { New-Item -ItemType Directory -Path $backupDir -Force | Out-Null }
$bakPath = Join-Path $backupDir 'OrchidCapital.bak'

# Update these if your host SQL instance or credentials differ
$SqlInstance = 'DESKTOP-GMTUJA5\\JAYESH'
$SqlUser = 'sa'
$SqlPass = 'sa@123'
$Database = 'OrchidCapital'

Write-Host "Backing up database $Database to $bakPath"
$sql = "BACKUP DATABASE [$Database] TO DISK = N'$bakPath' WITH INIT, SKIP, NOFORMAT;"

sqlcmd -S $SqlInstance -U $SqlUser -P $SqlPass -Q $sql

if ($LASTEXITCODE -eq 0) { Write-Host "Backup completed: $bakPath" } else { Write-Error "Backup failed (sqlcmd exit code $LASTEXITCODE)" }
