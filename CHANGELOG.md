# Changelog

All notable changes to ShiftPilot will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-05-21

### Added

#### Core Features
- User authentication with JWT tokens
- Shift management (CRUD operations)
- Sick leave request system with approval workflow
- Shift swap request functionality
- Employee availability tracking

#### API
- RESTful API with 5 main controllers
  - AuthController: Authentication endpoints
  - ShiftsController: Shift management
  - SickLeavesController: Sick leave management
  - ShiftSwapsController: Shift swap management
  - AvailabilityController: Availability tracking
  - UsersController: User management
  - SchedulingController: Scheduling and optimization

#### Services
- AuthService: User authentication and JWT generation
- ShiftService: Shift operations
- SickLeaveService: Sick leave management
- ShiftSwapService: Shift swap handling
- AvailabilityService: Availability tracking
- SchedulingService: Optimal schedule generation and conflict detection
- NotificationService: Notification system (skeleton)

#### Infrastructure
- Entity Framework Core database context
- Repository pattern for data access
- Dependency injection setup
- Exception handling middleware
- CORS configuration
- Swagger/OpenAPI documentation

#### Validators
- ShiftValidator: Shift time and date validation
- AvailabilityValidator: Availability validation
- SickLeaveValidator: Sick leave validation

#### Testing
- Unit test project with xUnit and Moq
- AuthServiceTests
- ShiftServiceTests
- SickLeaveServiceTests

#### Documentation
- README.md with setup instructions
- API_TESTING.md with Postman examples
- MIGRATIONS.md for database setup
- ROADMAP.md for future features
- CONTRIBUTING.md for developers
- CHANGELOG.md (this file)

#### DevOps
- Dockerfile for containerization
- docker-compose.yml for local development
- Environment configuration guide

### Technical Stack

- .NET 8.0
- ASP.NET Core
- Entity Framework Core 8.0
- SQL Server
- JWT Authentication
- xUnit testing framework
- Moq mocking library

### Database

- Users table
- Shifts table
- SickLeaves table
- ShiftSwapRequests table
- Availabilities table

## Upcoming

### [1.1.0] - Planned

- Email notification integration
- Push notifications
- Mobile app (React Native/Flutter)
- Enhanced logging and auditing
- Caching layer (Redis)
- Rate limiting
- API versioning

### [2.0.0] - Future

- AI-powered scheduling
- Revenue/visitor predictions
- AI chatbot assistant
- Advanced analytics
- Manager dashboard

### [3.0.0] - Long-term

- Camera integration
- Real-time occupancy analysis
- Autonomous optimization
