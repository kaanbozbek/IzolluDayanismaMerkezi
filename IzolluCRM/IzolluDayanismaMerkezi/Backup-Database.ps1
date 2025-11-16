# Database Backup Script
# Automatically backs up the SQLite database to DB_Backup folder

$ErrorActionPreference = "Stop"

# Configuration
$scriptPath = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Definition }
$dbPath = Join-Path $scriptPath "izolluvakfi.db"
$backupFolder = Join-Path $scriptPath "publish\DB_Backup"
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backupFileName = "izolluvakfi_backup_$timestamp.db"
$backupPath = Join-Path $backupFolder $backupFileName

# Create backup folder if it doesn't exist
if (-not (Test-Path $backupFolder)) {
    New-Item -ItemType Directory -Path $backupFolder | Out-Null
    Write-Host "[OK] Created backup folder: $backupFolder" -ForegroundColor Green
}

# Check if database exists
if (-not (Test-Path $dbPath)) {
    Write-Host "[ERROR] Database not found: $dbPath" -ForegroundColor Red
    exit 1
}

try {
    # Close any open SQLite connections (close WAL files)
    $walPath = "$dbPath-wal"
    $shmPath = "$dbPath-shm"
    
    # Copy main database file
    Copy-Item -Path $dbPath -Destination $backupPath -Force
    
    # Copy WAL file if exists
    if (Test-Path $walPath) {
        Copy-Item -Path $walPath -Destination "$backupPath-wal" -Force
    }
    
    # Copy SHM file if exists
    if (Test-Path $shmPath) {
        Copy-Item -Path $shmPath -Destination "$backupPath-shm" -Force
    }
    
    $backupSize = (Get-Item $backupPath).Length / 1KB
    Write-Host "[OK] Backup completed successfully!" -ForegroundColor Green
    Write-Host "  Location: $backupPath" -ForegroundColor Cyan
    Write-Host "  Size: $([math]::Round($backupSize, 2)) KB" -ForegroundColor Cyan
    Write-Host "  Time: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor Cyan
    
    # Clean up old backups (keep last 30 days)
    $oldBackups = Get-ChildItem -Path $backupFolder -Filter "izolluvakfi_backup_*.db" | 
                  Where-Object { $_.CreationTime -lt (Get-Date).AddDays(-30) }
    
    if ($oldBackups) {
        $oldBackups | Remove-Item -Force
        Write-Host "[OK] Cleaned up $($oldBackups.Count) old backup(s)" -ForegroundColor Yellow
    }
    
    # Show total backup count
    $totalBackups = (Get-ChildItem -Path $backupFolder -Filter "izolluvakfi_backup_*.db").Count
    Write-Host "  Total backups: $totalBackups" -ForegroundColor Cyan
    
    exit 0
}
catch {
    Write-Host "[ERROR] Backup failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
