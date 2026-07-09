# Restore the backup inside the mssql container mounted at /var/opt/mssql/backup
# Usage: run after docker compose up -d

$BackupFile = '/var/opt/mssql/backup/OrchidCapital.bak'
$Database = 'OrchidCapital'
$saPass = 'Sa@12345'

Write-Host "Running RESTORE FILELISTONLY to show logical file names..."
& sqlcmd -S "localhost,1433" -U "sa" -P $saPass -Q "RESTORE FILELISTONLY FROM DISK = N'$BackupFile'"

Write-Host "If the logical file names look correct you can run the following RESTORE command."
Write-Host "(The script will attempt a standard restore using common logical names.)"

$restoreCmd = "RESTORE DATABASE [$Database] FROM DISK = N'$BackupFile' WITH REPLACE, MOVE N'$Database' TO N'/var/opt/mssql/data/$Database.mdf', MOVE N'${Database}_log' TO N'/var/opt/mssql/data/${Database}_log.ldf'"
Write-Host "Executing: $restoreCmd"
& sqlcmd -S "localhost,1433" -U "sa" -P $saPass -Q $restoreCmd

Write-Host "Restore finished. If it failed, inspect the FILELISTONLY output and run a manual RESTORE with the correct MOVE names."
