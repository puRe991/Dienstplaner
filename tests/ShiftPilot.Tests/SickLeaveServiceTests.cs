using Xunit;
using Moq;
using ShiftPilot.API.Services;
using ShiftPilot.API.Repositories;
using ShiftPilot.Core.Models;

namespace ShiftPilot.Tests;

public class SickLeaveServiceTests
{
    private readonly Mock<ISickLeaveRepository> _sickLeaveRepositoryMock;
    private readonly SickLeaveService _sickLeaveService;

    public SickLeaveServiceTests()
    {
        _sickLeaveRepositoryMock = new Mock<ISickLeaveRepository>();
        _sickLeaveService = new SickLeaveService(_sickLeaveRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateSickLeaveAsync_ShouldSetPendingStatus()
    {
        // Arrange
        var sickLeave = new SickLeave
        {
            UserId = 1,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(1)
        };

        _sickLeaveRepositoryMock.Setup(r => r.AddSickLeaveAsync(It.IsAny<SickLeave>())).ReturnsAsync(sickLeave);

        // Act
        var result = await _sickLeaveService.CreateSickLeaveAsync(sickLeave);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(SickLeaveStatus.Pending, result.Status);
    }

    [Fact]
    public async Task ApproveSickLeaveAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var sickLeaveId = 1;
        var sickLeave = new SickLeave { Id = sickLeaveId, Status = SickLeaveStatus.Pending };

        _sickLeaveRepositoryMock.Setup(r => r.GetSickLeaveByIdAsync(sickLeaveId)).ReturnsAsync(sickLeave);
        _sickLeaveRepositoryMock.Setup(r => r.UpdateSickLeaveAsync(It.IsAny<SickLeave>())).ReturnsAsync(sickLeave);

        // Act
        var result = await _sickLeaveService.ApproveSickLeaveAsync(sickLeaveId);

        // Assert
        Assert.True(result);
        Assert.Equal(SickLeaveStatus.Approved, sickLeave.Status);
    }

    [Fact]
    public async Task RejectSickLeaveAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var sickLeaveId = 1;
        var sickLeave = new SickLeave { Id = sickLeaveId, Status = SickLeaveStatus.Pending };

        _sickLeaveRepositoryMock.Setup(r => r.GetSickLeaveByIdAsync(sickLeaveId)).ReturnsAsync(sickLeave);
        _sickLeaveRepositoryMock.Setup(r => r.UpdateSickLeaveAsync(It.IsAny<SickLeave>())).ReturnsAsync(sickLeave);

        // Act
        var result = await _sickLeaveService.RejectSickLeaveAsync(sickLeaveId);

        // Assert
        Assert.True(result);
        Assert.Equal(SickLeaveStatus.Rejected, sickLeave.Status);
    }
}
