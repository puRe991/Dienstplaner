using Xunit;
using Moq;
using ShiftPilot.API.Services;
using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.Tests;

public class ShiftServiceTests
{
    private readonly Mock<IShiftRepository> _shiftRepositoryMock;
    private readonly ShiftService _shiftService;

    public ShiftServiceTests()
    {
        _shiftRepositoryMock = new Mock<IShiftRepository>();
        _shiftService = new ShiftService(_shiftRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateShiftAsync_ShouldSetCorrectDefaults()
    {
        // Arrange
        var shift = new Shift
        {
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(8),
            Type = ShiftType.FullDay
        };

        _shiftRepositoryMock.Setup(r => r.AddShiftAsync(It.IsAny<Shift>())).ReturnsAsync(shift);

        // Act
        var result = await _shiftService.CreateShiftAsync(shift);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ShiftStatus.Unassigned, result.Status);
    }

    [Fact]
    public async Task AssignShiftAsync_WithValidShift_ShouldReturnTrue()
    {
        // Arrange
        var shiftId = 1;
        var userId = 1;
        var shift = new Shift { Id = shiftId, Status = ShiftStatus.Unassigned };

        _shiftRepositoryMock.Setup(r => r.GetShiftByIdAsync(shiftId)).ReturnsAsync(shift);
        _shiftRepositoryMock.Setup(r => r.UpdateShiftAsync(It.IsAny<Shift>())).ReturnsAsync(shift);

        // Act
        var result = await _shiftService.AssignShiftAsync(shiftId, userId);

        // Assert
        Assert.True(result);
        Assert.Equal(userId, shift.AssignedUserId);
        Assert.Equal(ShiftStatus.Assigned, shift.Status);
    }

    [Fact]
    public async Task AssignShiftAsync_WithInvalidShift_ShouldReturnFalse()
    {
        // Arrange
        var shiftId = 999;
        var userId = 1;

        _shiftRepositoryMock.Setup(r => r.GetShiftByIdAsync(shiftId)).ReturnsAsync((Shift?)null);

        // Act
        var result = await _shiftService.AssignShiftAsync(shiftId, userId);

        // Assert
        Assert.False(result);
    }
}
