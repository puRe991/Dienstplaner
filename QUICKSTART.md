# Quick Start Guide

## Automatic Startup (Recommended)

### Windows

1. Open Command Prompt as Administrator
2. Navigate to the project directory
3. Run the startup script:

```bash
start.bat
```

The script will:
- ✅ Check all prerequisites (Docker, .NET SDK)
- ✅ Start the SQL Server database
- ✅ Restore NuGet packages
- ✅ Build the solution
- ✅ Run database migrations
- ✅ Run unit tests (optional)
- ✅ Start the API server

### Linux / macOS

1. Open Terminal
2. Navigate to the project directory
3. Make the script executable and run it:

```bash
chmod +x start.sh
./start.sh
```

## Development Setup

### First Time Setup (Windows)

```bash
setup-dev.bat
```

### First Time Setup (Linux/macOS)

```bash
chmod +x setup-dev.sh
./setup-dev.sh
```

This will:
- Install .NET 8.0 (if needed)
- Install Docker (if needed)
- Configure VS Code extensions
- Create environment files
- Restore packages

## Manual Setup

If you prefer to set up manually:

### 1. Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

### 2. Clone Repository

```bash
git clone https://github.com/puRe991/Dienstplaner.git
cd Dienstplaner
git checkout feature/mvp-setup
```

### 3. Start Database

```bash
docker-compose up -d db
```

Wait 30 seconds for the database to initialize.

### 4. Build and Restore

```bash
dotnet restore
dotnet build --configuration Release
```

### 5. Run Migrations

```bash
cd src/ShiftPilot.API
dotnet ef database update
cd ../..
```

### 6. Run Tests (Optional)

```bash
dotnet test tests/ShiftPilot.Tests
```

### 7. Start API

```bash
cd src/ShiftPilot.API
dotnet run
```

## Access the Application

Once the API is running:

- **API Base URL**: `https://localhost:5001` or `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001/swagger`
- **Database**: `localhost:1433` (SQL Server)

## Stopping the Application

### Windows

```bash
stop.bat
```

### Linux/macOS

```bash
chmod +x stop.sh
./stop.sh
```

This will stop and remove all Docker containers.

## Troubleshooting

### Database Connection Issues

If the database fails to start:

```bash
# Check Docker containers
docker ps -a

# View logs
docker logs dienstplaner-db-1

# Rebuild containers
docker-compose down -v
docker-compose up -d db
```

### Port Already in Use

If port 5000 or 5001 is in use:

```bash
# Linux/macOS
lsof -i :5001

# Windows
netstat -ano | findstr :5001
```

### Clear NuGet Cache

```bash
dotnet nuget locals all --clear
dotnet restore
```

## Environment Variables

Edit `.env` or `appsettings.json` to configure:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ShiftPilotDB;..."
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "ExpirationMinutes": 1440
  }
}
```

## IDE Setup

### Visual Studio 2022

1. Open `ShiftPilot.sln`
2. Set `ShiftPilot.API` as startup project
3. Press `F5` to run

### VS Code

1. Install extensions:
   - C# Dev Kit
   - Database Clients
2. Open folder
3. Press `F5` to debug

### JetBrains Rider

1. Open `ShiftPilot.sln`
2. Configure run configuration
3. Click Run

## Docker Compose Services

The `docker-compose.yml` includes:

- **db**: SQL Server 2022
  - Port: 1433
  - User: sa
  - Password: YourPassword123!

- **api**: ShiftPilot API (optional)
  - Port: 5000, 5001

## Next Steps

1. ✅ Run the application
2. 📖 Read [API_TESTING.md](API_TESTING.md)
3. 🔍 Check out [ADVANCED_FEATURES.md](ADVANCED_FEATURES.md)
4. 📈 Review [PERFORMANCE_OPTIMIZATION.md](PERFORMANCE_OPTIMIZATION.md)
5. 👥 See [CONTRIBUTING.md](CONTRIBUTING.md)

## Support

For issues or questions:

1. Check [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
2. Search existing GitHub issues
3. Create a new issue with details
