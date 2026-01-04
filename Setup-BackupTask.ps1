# Setup Scheduled Task for Database Backup
# Automatically creates a Windows Task Scheduler task to run daily backups

$ErrorActionPreference = "Stop"

# Configuration
$scriptPath = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Definition }
$backupScriptPath = Join-Path $scriptPath "Backup-Database.ps1"
$taskName = "IzolluVakfi_Daily_Database_Backup"
$taskDescription = "Automatically backs up Izollu Vakfi CRM database daily at 2:00 AM"
$taskTime = "02:00"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Izollu Vakfi CRM - Backup Task Setup" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Check if backup script exists
if (-not (Test-Path $backupScriptPath)) {
    Write-Host "[ERROR] Backup script not found: $backupScriptPath" -ForegroundColor Red
    Write-Host "  Please ensure Backup-Database.ps1 is in the same folder." -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

# Check if running as Administrator
$isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdmin) {
    Write-Host "[ERROR] This script requires Administrator privileges!" -ForegroundColor Red
    Write-Host "  Please run PowerShell as Administrator and try again." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "To run as Administrator:" -ForegroundColor Cyan
    Write-Host "  1. Right-click PowerShell" -ForegroundColor White
    Write-Host "  2. Select 'Run as Administrator'" -ForegroundColor White
    Write-Host "  3. Run this script again" -ForegroundColor White
    Read-Host "`nPress Enter to exit"
    exit 1
}

try {
    # Check if task already exists
    $existingTask = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue
    
    if ($existingTask) {
        Write-Host "[WARNING] Task '$taskName' already exists!" -ForegroundColor Yellow
        $response = Read-Host "Do you want to remove and recreate it? (Y/N)"
        
        if ($response -eq 'Y' -or $response -eq 'y') {
            Unregister-ScheduledTask -TaskName $taskName -Confirm:$false
            Write-Host "[OK] Existing task removed" -ForegroundColor Green
        }
        else {
            Write-Host "[INFO] Setup cancelled" -ForegroundColor Yellow
            Read-Host "Press Enter to exit"
            exit 0
        }
    }
    
    # Create scheduled task action
    $action = New-ScheduledTaskAction -Execute "PowerShell.exe" `
        -Argument "-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File `"$backupScriptPath`"" `
        -WorkingDirectory $scriptPath
    
    # Create scheduled task trigger (daily at specified time)
    $trigger = New-ScheduledTaskTrigger -Daily -At $taskTime
    
    # Create scheduled task settings
    $settings = New-ScheduledTaskSettingsSet `
        -AllowStartIfOnBatteries `
        -DontStopIfGoingOnBatteries `
        -StartWhenAvailable `
        -RunOnlyIfNetworkAvailable:$false `
        -MultipleInstances IgnoreNew
    
    # Create scheduled task principal (run with highest privileges)
    $principal = New-ScheduledTaskPrincipal `
        -UserId "$env:USERDOMAIN\$env:USERNAME" `
        -LogonType Interactive `
        -RunLevel Highest
    
    # Register the scheduled task
    Register-ScheduledTask `
        -TaskName $taskName `
        -Description $taskDescription `
        -Action $action `
        -Trigger $trigger `
        -Settings $settings `
        -Principal $principal | Out-Null
    
    Write-Host "[OK] Scheduled task created successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Task Details:" -ForegroundColor Cyan
    Write-Host "  Name: $taskName" -ForegroundColor White
    Write-Host "  Schedule: Daily at $taskTime" -ForegroundColor White
    Write-Host "  Script: $backupScriptPath" -ForegroundColor White
    Write-Host "  Status: Enabled" -ForegroundColor White
    Write-Host ""
    
    # Test run option
    $testRun = Read-Host "Do you want to run a test backup now? (Y/N)"
    
    if ($testRun -eq 'Y' -or $testRun -eq 'y') {
        Write-Host ""
        Write-Host "Running test backup..." -ForegroundColor Cyan
        Write-Host ""
        
        # Run the backup script
        & $backupScriptPath
        
        Write-Host ""
        Write-Host "[OK] Test backup completed!" -ForegroundColor Green
    }
    
    Write-Host ""
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host "Setup Complete!" -ForegroundColor Green
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Your database will be backed up automatically every day at $taskTime" -ForegroundColor White
    Write-Host "Backups are stored in: $(Join-Path $scriptPath 'publish\DB_Backup')" -ForegroundColor White
    Write-Host "Old backups (30+ days) are automatically deleted" -ForegroundColor White
    Write-Host ""
    Write-Host "To manage the task:" -ForegroundColor Cyan
    Write-Host "  - Open Task Scheduler (taskschd.msc)" -ForegroundColor White
    Write-Host "  - Look for '$taskName'" -ForegroundColor White
    Write-Host ""
    
    Read-Host "Press Enter to exit"
    
}
catch {
    Write-Host "[ERROR] Failed to create scheduled task: $($_.Exception.Message)" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}
