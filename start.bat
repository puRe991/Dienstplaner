@echo off
REM ShiftPilot Startup Script for Windows
REM This script sets up and starts the entire ShiftPilot application

setlocal enabledelayedexpansion

echo ===============================================
echo    ^!^!^! ShiftPilot Startup Script - Windows   ^!^!^!
echo ===============================================
echo.

REM Color codes using Windows command line
REM Note: Requires Windows 10+ for ANSI support or use alternative method

REM Step 1: Check prerequisites
echo [i] Checking prerequisites...
echo.

where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo [X] .NET SDK is not installed
    echo Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)
echo [OK] .NET SDK found

where docker >nul 2>nul
if %errorlevel% neq 0 (
    echo [X] Docker is not installed
    echo Please install Docker from https://www.docker.com/products/docker-desktop
    pause
    exit /b 1
)
echo [OK] Docker found

where docker-compose >nul 2>nul
if %errorlevel% neq 0 (
    echo [X] Docker Compose is not installed
    echo Please install Docker Compose from https://docs.docker.com/compose/install
    pause
    exit /b 1
)
echo [OK] Docker Compose found

REM Step 2: Check if in git repository
echo.
echo [i] Checking repository...
echo.

if not exist ".git" (
    echo [X] Not in a git repository directory
    pause
    exit /b 1
)
echo [OK] Git repository found

REM Step 3: Start database with Docker Compose
echo.
echo [i] Starting database with Docker Compose...
echo.

docker-compose up -d db
if %errorlevel% neq 0 (
    echo [X] Failed to start database container
    pause
    exit /b 1
)
echo [OK] Database container started

REM Wait for database to be ready
echo [i] Waiting for database to be ready (30 seconds)...
timeout /t 30 /nobreak
echo [OK] Database is ready

REM Step 4: Restore NuGet packages
echo.
echo [i] Restoring NuGet packages...
echo.

dotnet restore >nul 2>&1
if %errorlevel% neq 0 (
    echo [X] Failed to restore packages
    pause
    exit /b 1
)
echo [OK] Packages restored successfully

REM Step 5: Build the solution
echo.
echo [i] Building ShiftPilot solution...
echo.

dotnet build --configuration Release >nul 2>&1
if %errorlevel% neq 0 (
    echo [X] Failed to build solution
    pause
    exit /b 1
)
echo [OK] Solution built successfully

REM Step 6: Run database migrations
echo.
echo [i] Running database migrations...
echo.

cd src\ShiftPilot.API
dotnet ef database update >nul 2>&1
if %errorlevel% neq 0 (
    echo [X] Failed to run migrations
    cd ..\..
    pause
    exit /b 1
)
echo [OK] Database migrations completed
cd ..\..

REM Step 7: Run tests (optional)
echo.
set /p run_tests="Do you want to run unit tests? (y/n): "
if /i "%run_tests%"=="y" (
    echo [i] Running unit tests...
    echo.
    dotnet test tests\ShiftPilot.Tests --verbosity minimal >nul 2>&1
    if %errorlevel% neq 0 (
        echo [X] Some tests failed
        echo Run 'dotnet test tests\ShiftPilot.Tests' for details
    ) else (
        echo [OK] All tests passed
    )
)

REM Step 8: Start the API
echo.
echo [i] Starting ShiftPilot API...
echo.
echo The API will start on:
echo   https://localhost:5001
echo   http://localhost:5000
echo.
echo Swagger UI will be available at:
echo   https://localhost:5001/swagger
echo.
echo [i] Press Ctrl+C to stop the API
echo.

cd src\ShiftPilot.API
dotnet run

pause
