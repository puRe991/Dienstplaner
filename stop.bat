@echo off
REM ShiftPilot Stop Script for Windows
REM This script stops all running ShiftPilot services

echo ===============================================
echo    ^!^!^! ShiftPilot Stop Script - Windows    ^!^!^!
echo ===============================================
echo.

echo [i] Stopping Docker containers...
echo.

docker-compose down
if %errorlevel% neq 0 (
    echo [X] Failed to stop containers
    pause
    exit /b 1
)
echo [OK] All containers stopped successfully

echo.
echo [i] Removing volumes (optional)...
set /p remove_volumes="Do you want to remove database volumes? (y/n): "
if /i "%remove_volumes%"=="y" (
    docker-compose down -v
    echo [OK] Volumes removed
)

echo.
echo [OK] ShiftPilot has been stopped
pause
