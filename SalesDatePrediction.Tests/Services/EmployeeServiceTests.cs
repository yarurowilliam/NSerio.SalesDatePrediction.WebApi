using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;
using SalesDatePrediction.Core.Services;

namespace SalesDatePrediction.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepository;
        private readonly Mock<ILogger<EmployeeService>> _mockLogger;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _mockRepository = new Mock<IEmployeeRepository>();
            _mockLogger = new Mock<ILogger<EmployeeService>>();
            _service = new EmployeeService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenEmployeesExist_ReturnsEmployees()
        {
            // Arrange
            var expectedEmployees = new List<Employee>
        {
            new() { EmpId = 1, FullName = "John Doe" },
            new() { EmpId = 2, FullName = "Jane Smith" }
        };

            _mockRepository.Setup(x => x.GetAllAsync())
                          .ReturnsAsync(expectedEmployees);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedEmployees);
            _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoEmployees_ReturnsEmptyList()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetAllAsync())
                          .ReturnsAsync(new List<Employee>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryThrows_LogsAndRethrows()
        {
            // Arrange
            var expectedException = new Exception("Database error");
            _mockRepository.Setup(x => x.GetAllAsync())
                          .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _service.GetAllAsync());

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
