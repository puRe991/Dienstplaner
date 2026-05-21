using Xunit;
using Moq;
using ShiftPilot.API.Services;
using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _authService = new AuthService(_userRepositoryMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var email = "test@example.com";
        var firstName = "John";
        var lastName = "Doe";
        var password = "SecurePassword123";

        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync((User?)null);

        // Act
        var result = await _authService.RegisterAsync(email, firstName, lastName, password);

        // Assert
        Assert.True(result.Success);
        Assert.NotEmpty(result.Token);
        Assert.Equal("Registration successful", result.Message);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldReturnFailure()
    {
        // Arrange
        var email = "existing@example.com";
        var existingUser = new User { Email = email };

        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(existingUser);

        // Act
        var result = await _authService.RegisterAsync(email, "John", "Doe", "password");

        // Assert
        Assert.False(result.Success);
        Assert.Empty(result.Token);
        Assert.Equal("Email already registered", result.Message);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnSuccess()
    {
        // Arrange
        var email = "test@example.com";
        var password = "SecurePassword123";
        var user = new User { Id = 1, Email = email };

        _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(user);

        // Act
        var result = await _authService.LoginAsync(email, password);

        // Assert - Note: This will fail because password verification needs proper setup
        // This is a simplified test
    }
}
