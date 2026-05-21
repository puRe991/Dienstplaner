# ShiftPilot - AI-Powered Staff Management Platform

An intelligent shift planning and staff management system for retail businesses, designed to reduce chaos, minimize paperwork, and improve employee satisfaction.

## Features

### MVP (Version 1)
- **Shift Planning**: Easy shift creation and assignment
- **Sick Leave Management**: Submit and approve sick leave requests
- **Shift Swaps**: Employees can request to swap shifts
- **Availability Tracking**: Track employee availability
- **User Management**: Employee and manager roles
- **JWT Authentication**: Secure API with token-based authentication

## Project Structure

```
ShiftPilot/
├── src/
│   ├── ShiftPilot.API/          # ASP.NET Core Web API
│   ├── ShiftPilot.Core/         # Core models and interfaces
│   └── ShiftPilot.Data/         # Entity Framework Core DbContext
├── ShiftPilot.sln               # Solution file
└── README.md                    # This file
```

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core 8.0
- **Authentication**: JWT (JSON Web Tokens)
- **Architecture**: Repository Pattern with Service Layer

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/puRe991/Dienstplaner.git
   cd Dienstplaner
   ```

2. **Update connection string** in `src/ShiftPilot.API/appsettings.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Your connection string here"
   }
   ```

3. **Apply database migrations**
   ```bash
   cd src/ShiftPilot.API
   dotnet ef database update -s ShiftPilot.API.csproj -p ../ShiftPilot.Data/ShiftPilot.Data.csproj
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the API**
   - Swagger UI: `https://localhost:5001/swagger`
   - Base API URL: `https://localhost:5001/api`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and get JWT token

### Shifts
- `GET /api/shifts` - Get all shifts
- `GET /api/shifts/{id}` - Get shift by ID
- `GET /api/shifts/user/{userId}` - Get shifts for a user
- `GET /api/shifts/unassigned` - Get unassigned shifts
- `POST /api/shifts` - Create a new shift
- `PUT /api/shifts/{id}` - Update a shift
- `DELETE /api/shifts/{id}` - Delete a shift
- `POST /api/shifts/{id}/assign/{userId}` - Assign shift to user

### Sick Leave
- `GET /api/sickleaves` - Get all sick leave requests
- `GET /api/sickleaves/{id}` - Get sick leave by ID
- `GET /api/sickleaves/user/{userId}` - Get sick leaves for a user
- `POST /api/sickleaves` - Create new sick leave request
- `PUT /api/sickleaves/{id}` - Update sick leave request
- `DELETE /api/sickleaves/{id}` - Delete sick leave request
- `POST /api/sickleaves/{id}/approve` - Approve sick leave
- `POST /api/sickleaves/{id}/reject` - Reject sick leave

### Shift Swaps
- `GET /api/shiftswaps` - Get all swap requests
- `GET /api/shiftswaps/{id}` - Get swap request by ID
- `GET /api/shiftswaps/pending/{userId}` - Get pending swaps for user
- `POST /api/shiftswaps` - Create new swap request
- `POST /api/shiftswaps/{id}/approve` - Approve swap request
- `POST /api/shiftswaps/{id}/reject` - Reject swap request

### Availability
- `GET /api/availability` - Get all availability records
- `GET /api/availability/{id}` - Get availability by ID
- `GET /api/availability/user/{userId}` - Get availabilities for a user
- `GET /api/availability/range` - Get availabilities for date range
- `POST /api/availability` - Create availability record
- `PUT /api/availability/{id}` - Update availability record
- `DELETE /api/availability/{id}` - Delete availability record

## Database Models

### User
- Stores employee and manager information
- Roles: Employee, Manager, Admin

### Shift
- Represents work shifts
- Types: Morning, Afternoon, Evening, Night, FullDay
- Status: Unassigned, Assigned, Completed, Cancelled

### SickLeave
- Tracks sick leave requests
- Status: Pending, Approved, Rejected, Cancelled

### ShiftSwapRequest
- Manages shift swap requests between employees
- Status: Pending, Approved, Rejected, Cancelled

### Availability
- Tracks employee availability windows
- Includes date, start time, end time

## Future Enhancements

### Version 2
- [ ] Automatic replacement suggestions
- [ ] Revenue/visitor predictions
- [ ] AI chatbot assistant

### Version 3
- [ ] Camera integration for attendance
- [ ] Live occupancy analysis
- [ ] Autonomous optimization suggestions

## Contributing

Contributions are welcome! Please follow these steps:

1. Create a feature branch (`git checkout -b feature/amazing-feature`)
2. Commit your changes (`git commit -m 'Add amazing feature'`)
3. Push to the branch (`git push origin feature/amazing-feature`)
4. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support, please open an issue on the GitHub repository.
