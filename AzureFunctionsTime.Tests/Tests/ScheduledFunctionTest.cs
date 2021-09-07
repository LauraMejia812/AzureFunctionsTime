using AzureFunctionsTime.Functions.Functions;
using AzureFunctionsTime.Tests.Helpers;
using System;

using Xunit;

namespace AzureFunctionsTime.Tests.Tests
{
    public class ScheduledFunctionTest
    {
        [Fact]
        public void ScheduledFunction_Should_Log_Message()
        {
            //Arrange
            MockCloudTableTimes mockTimes = new MockCloudTableTimes(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            ListLogger logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);

            //Act
            ScheduledFunction.Run(null, mockTimes, logger);
            string message = logger.logs[0];

            //Assert
            Assert.Contains("Deleting completed time", message);
        }
    }
}
