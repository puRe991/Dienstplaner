@echo off
REM ShiftPilot Development Setup Script for Windows
REM This script sets up the development environment

setlocal enabledelayedexpansion

echo ===============================================
echo    ShiftPilot Dev Setup - Windows
echo ===============================================
echo.

REM Step 1: Check .NET 8.0
echo [i] Checking .NET 8.0 SDK...
echo.

where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo [X] .NET SDK not found
    echo Please download from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)
echo [OK] .NET SDK found

REM Step 2: Check Docker
echo.
echo [i] Checking Docker...
echo.

where docker >nul 2>nul
if %errorlevel% neq 0 (
    echo [X] Docker not found
    echo Please download Docker Desktop from https://www.docker.com/products/docker-desktop
    pause
    exit /b 1
)
echo [OK] Docker found

REM Step 3: Create environment files
echo.
echo [i] Creating environment configuration files...
echo.

if not exist ".env" (
    copy .env.example .env
    echo [OK] .env file created (update with your values)
) else (
    echo [i] .env file already exists
)

REM Step 4: Restore packages
echo.
echo [i] Restoring NuGet packages...
echo.

dotnet restore >nul 2>&1
if %errorlevel% neq 0 (
    echo [X] Failed to restore packages
    pause
    exit /b 1
)
echo [OK] Packages restored

REM Step 5: Summary
echo.
echo ===============================================
echo    ^!^!^! Setup complete!                    ^!^!^!
echo ===============================================
echo.
echo Next steps:
echo 1. Update .env file with your configuration
echo 2. Run 'start.bat' to start the application
echo 3. Visit https://localhost:5001/swagger for API docs
echo.
pause
