# Contributing Guidelines

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/YOUR_USERNAME/Dienstplaner.git`
3. Create a feature branch: `git checkout -b feature/your-feature-name`
4. Follow the coding standards below
5. Commit your changes: `git commit -m 'feat: Add your feature'`
6. Push to your fork: `git push origin feature/your-feature-name`
7. Create a Pull Request

## Coding Standards

### C# Guidelines

- **Naming**: PascalCase for classes, methods, properties; camelCase for parameters and local variables
- **Access Modifiers**: Always specify (public, private, protected)
- **LINQ**: Use method syntax over query syntax
- **Async/Await**: Use `async Task` for void operations that are async
- **Error Handling**: Use specific exception types, avoid catching generic Exception
- **Comments**: Use XML documentation for public methods

### File Structure

```
src/
├── ShiftPilot.API/
│   ├── Controllers/          # API endpoints
│   ├── Services/             # Business logic
│   ├── Repositories/         # Data access
│   ├── Models/               # Request/Response models
│   ├── DTOs/                 # Data transfer objects
│   ├── Validators/           # Validation logic
│   ├── Middleware/           # Custom middleware
│   ├── Extensions/           # Extension methods
│   ├── Constants/            # Application constants
│   └── Program.cs            # Application entry point
├── ShiftPilot.Core/          # Domain models
└── ShiftPilot.Data/          # Database context and migrations
```

## Commit Message Format

```
<type>: <subject>

<body>

<footer>
```

### Types

- **feat**: A new feature
- **fix**: A bug fix
- **docs**: Documentation changes
- **style**: Code style changes (formatting, etc.)
- **refactor**: Code refactoring
- **perf**: Performance improvements
- **test**: Adding or updating tests
- **chore**: Maintenance tasks

### Examples

```
feat: Add shift swap approval notification

Implement email notification when a shift swap request is approved.
Includes NotificationService integration with shift swap workflow.

Closes #123
```

## Pull Request Process

1. Update documentation if needed
2. Add tests for new features
3. Ensure all tests pass: `dotnet test`
4. Update CHANGELOG.md
5. Get at least 2 approvals from maintainers
6. Squash commits if needed

## Testing

### Running Tests

```bash
cd tests/ShiftPilot.Tests
dotnet test
```

### Test Coverage

- Aim for >80% code coverage
- Test happy paths and error scenarios
- Use Arrange-Act-Assert pattern

## Code Review Checklist

- [ ] Code follows naming conventions
- [ ] No hardcoded values (use constants)
- [ ] Error handling is appropriate
- [ ] No console.WriteLine (use ILogger)
- [ ] Database queries are optimized
- [ ] Security considerations are addressed
- [ ] Tests are included and passing
- [ ] Documentation is updated

## Security Guidelines

- Never commit secrets or passwords
- Use parameterized queries to prevent SQL injection
- Validate and sanitize user input
- Use HTTPS in production
- Keep dependencies updated
- Review OWASP top 10

## Performance Guidelines

- Use async/await for I/O operations
- Implement caching for frequently accessed data
- Use proper indexing in database
- Avoid N+1 queries
- Profile before and after optimization

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

## Questions?

Open an issue or contact the maintainers.
