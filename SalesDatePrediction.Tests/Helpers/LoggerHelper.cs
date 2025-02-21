using Microsoft.Extensions.Logging;
using Moq;

namespace SalesDatePrediction.Tests.Helpers;

public static class LoggerHelper
{
    public static ILogger<T> CreateLogger<T>()
    {
        return new Mock<ILogger<T>>().Object;
    }
}
