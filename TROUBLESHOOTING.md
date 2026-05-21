# Troubleshooting Guide

## Common Issues and Solutions

## Database Issues

### Issue: Database Connection Timeout

**Error**: `timeout expired` or `Connection refused`

**Solutions**:

1. Ensure Docker is running:
   ```bash
   docker ps
   ```

2. Check if SQL Server container is running:
   ```bash
   docker ps | grep mssql
   ```

3. Restart the database:
   ```bash
   docker-compose down
   docker-compose up -d db
   sleep 30  # Wait for database to start
   ```

4. Check database logs:
   ```bash
   docker logs dienstplaner-db-1
   ```

5. Verify connection string in `appsettings.json`:
   ```json
   "DefaultConnection": "Server=localhost;Database=ShiftPilotDB;User Id=sa;Password=YourPassword123;TrustServerCertificate=true;"
   ```

### Issue: "Cannot create database"

**Error**: `CREATE DATABASE failed because database already exists`

**Solutions**:

1. Drop the existing database:
   ```bash
   docker-compose down -v
   docker-compose up -d db
   ```

2. Or manually drop it via SQL:
   ```sql
   DROP DATABASE ShiftPilotDB;
   ```

## Migration Issues

### Issue: Migration Failed

**Error**: `Pending model changes detected`

**Solutions**:

1. Create a new migration:
   ```bash
   cd src/ShiftPilot.API
   dotnet ef migrations add {MigrationName} -p ../ShiftPilot.Data -s ShiftPilot.API.csproj
   ```

2. Review the migration file in `ShiftPilot.Data/Migrations/`

3. Apply the migration:
   ```bash
   dotnet ef database update
   ```

4. If something went wrong, rollback:
   ```bash
   dotnet ef database update {PreviousMigrationName}
   ```

## API Issues

### Issue: Port Already in Use

**Error**: `Address already in use` on port 5001 or 5000

**Windows Solutions**:

```bash
# Find process using the port
netstat -ano | findstr :5001

# Kill the process (replace PID)
taskkill /PID {PID} /F

# Or use a different port in launchSettings.json
```

**Linux/macOS Solutions**:

```bash
# Find process using the port
lsof -i :5001

# Kill the process
kill -9 {PID}
```

### Issue: HTTPS Certificate Error

**Error**: `The certificate is invalid or untrusted`

**Solutions**:

1. Trust .NET HTTPS certificate:
   ```bash
   dotnet dev-certs https --trust
   ```

2. For macOS, you may need to accept the certificate in Keychain

3. In development, use HTTP instead:
   ```bash
   # Modify launchSettings.json
   "applicationUrl": "http://localhost:5000"
   ```

## Build Issues

### Issue: "Project file does not exist"

**Error**: Cannot find `.csproj` files

**Solutions**:

1. Ensure you're in the correct directory:
   ```bash
   pwd  # Should show .../Dienstplaner
   ls -la  # Should show ShiftPilot.sln
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Clean and rebuild:
   ```bash
   dotnet clean
   dotnet build
   ```

### Issue: Version Mismatch

**Error**: `Target framework not found` or `SDK version mismatch`

**Solutions**:

1. Check .NET SDK version:
   ```bash
   dotnet --version
   ```

2. Install .NET 8.0:
   ```bash
   # Windows (using winget)
   winget install Microsoft.DotNet.SDK.8
   
   # Linux/macOS
   curl -fsSL https://dot.net/v1/dotnet-install.sh | bash -- --version 8.0
   ```

3. List all installed SDKs:
   ```bash
   dotnet --list-sdks
   ```

## Test Issues

### Issue: Tests Won't Run

**Error**: `Could not find test sources`

**Solutions**:

1. Ensure test project file exists:
   ```bash
   ls tests/ShiftPilot.Tests/ShiftPilot.Tests.csproj
   ```

2. Restore test dependencies:
   ```bash
   cd tests/ShiftPilot.Tests
   dotnet restore
   cd ../..
   ```

3. Run with verbose output:
   ```bash
   dotnet test --verbosity detailed
   ```

### Issue: Database Tests Fail

**Error**: `No DbContext could be found`

**Solutions**:

1. Ensure test database is running
2. Use in-memory database for tests:
   ```csharp
   services.AddDbContext<ApplicationDbContext>(options =>
       options.UseInMemoryDatabase("TestDb"));
   ```

## Docker Issues

### Issue: Docker Daemon Not Running

**Error**: `Cannot connect to the Docker daemon`

**Solutions**:

**Windows**:
- Start Docker Desktop application

**Linux**:
```bash
sudo systemctl start docker
```

**macOS**:
- Click Docker icon in applications

### Issue: Container Won't Start

**Error**: `Error response from daemon`

**Solutions**:

```bash
# Remove all containers and images
docker system prune -a

# Rebuild
docker-compose build --no-cache
docker-compose up -d
```

### Issue: Volume Mount Error

**Error**: `Mount denied` or `Permission denied`

**Solutions**:

**Windows**:
- Right-click Docker icon → Settings → Resources → File Sharing
- Add your project directory

**Linux**:
```bash
sudo usermod -aG docker $USER
newgrp docker
```

## Authentication Issues

### Issue: Invalid JWT Token

**Error**: `401 Unauthorized` on protected endpoints

**Solutions**:

1. Register a new user:
   ```bash
   POST /api/auth/register
   {
     "email": "test@example.com",
     "firstName": "Test",
     "lastName": "User",
     "password": "Password123!"
   }
   ```

2. Copy the token from response

3. Add to headers:
   ```
   Authorization: Bearer {token}
   ```

4. Check token expiration time in JWT claims

## Performance Issues

### Issue: Slow API Responses

**Solutions**:

1. Check database performance:
   ```sql
   -- Enable statistics
   SET STATISTICS IO ON;
   SET STATISTICS TIME ON;
   ```

2. Clear memory cache:
   ```bash
   # Restart the API
   # Cache is automatically cleared on restart
   ```

3. Check SQL Server:
   ```bash
   docker exec -it {container_id} sqlcmd -S localhost -U sa -P YourPassword123!
   ```

## Log Files

Check logs for detailed error information:

```bash
# API logs
cd src/ShiftPilot.API
ls logs/  # If configured

# Docker logs
docker logs -f dienstplaner-db-1
docker logs -f dienstplaner-api-1
```

## Getting Help

1. **Check existing issues**: https://github.com/puRe991/Dienstplaner/issues
2. **Read documentation**: Check CONTRIBUTING.md and ADVANCED_FEATURES.md
3. **Enable verbose logging**: Set LogLevel to "Debug"
4. **Ask on GitHub**: Create a new issue with details and logs

## Recovery Procedures

### Complete Reset

```bash
# Stop all containers
docker-compose down -v

# Remove NuGet cache
dotnet nuget locals all --clear

# Start fresh
./start.sh  # or start.bat on Windows
```

### Database Reset Only

```bash
# Keep containers, reset database
docker exec -it {container_id} sqlcmd -S localhost -U sa -P YourPassword123! -Q "DROP DATABASE ShiftPilotDB;"

# Run migrations again
dotnet ef database update
```

### Rebuild Everything

```bash
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```
