# Migration Guide

## Running Database Migrations

### Initial Setup

1. Open the Package Manager Console in Visual Studio
2. Set the Default Project to `ShiftPilot.Data`
3. Run the following command:

```powershell
Update-Database -Startup ShiftPilot.API
```

### Using dotnet CLI

```bash
cd src/ShiftPilot.API
dotnet ef database update
```

### Creating a New Migration

If you modify the database models, create a new migration:

```bash
dotnet ef migrations add YourMigrationName -p ../ShiftPilot.Data -s ShiftPilot.API.csproj
```

Then apply it:

```bash
dotnet ef database update
```

## Database Schema

The database consists of the following tables:

- **Users**: Employee and manager information
- **Shifts**: Work shift schedules
- **SickLeaves**: Sick leave requests and approvals
- **ShiftSwapRequests**: Shift exchange requests
- **Availabilities**: Employee availability windows

## Rollback

To rollback to a previous migration:

```bash
dotnet ef database update PreviousMigrationName
```
