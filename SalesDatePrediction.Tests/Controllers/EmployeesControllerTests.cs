using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SalesDatePrediction.API.Controllers;
using SalesDatePrediction.Core.Interfaces;
using SalesDatePrediction.Core.Models;

namespace SalesDatePrediction.Tests.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IEmployeeService> _mockService;
        private readonly Mock<ILogger<EmployeesController>> _mockLogger;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _mockService = new Mock<IEmployeeService>();
            _mockLogger = new Mock<ILogger<EmployeesController>>();
            _controller = new EmployeesController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAll_WhenEmployeesExist_ReturnsOkWithEmployees()
        {
            // Arrange
            var expectedEmployees = new List<Employee>
        {
            new() { EmpId = 1, FullName = "John Doe" },
            new() { EmpId = 2, FullName = "Jane Smith" }
        };

            _mockService.Setup(x => x.GetAllAsync())
                       .ReturnsAsync(expectedEmployees);

            // Act
            var actionResult = await _controller.GetAll();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expectedEmployees);
        }

        [Fact]
        public async Task GetAll_WhenNoEmployees_ReturnsOkWithEmptyList()
        {
            // Arrange
            _mockService.Setup(x => x.GetAllAsync())
                       .ReturnsAsync(new List<Employee>());

            // Act
            var actionResult = await _controller.GetAll();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            var employees = result!.Value as IEnumerable<Employee>;
            employees.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_WhenServiceThrows_ReturnsInternalServerError()
        {
            // Arrange
            _mockService.Setup(x => x.GetAllAsync())
                       .ThrowsAsync(new Exception("Service error"));

            // Act
            var actionResult = await _controller.GetAll();

            // Assert
            var result = actionResult.Result as ObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

            var error = result.Value as ErrorResponse;
            error.Should().NotBeNull();
            error!.Message.Should().Be("An error occurred while processing your request");
        }
    }
}
