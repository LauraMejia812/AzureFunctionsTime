using AzureFunctionsTime.common.Models;
using AzureFunctionsTime.Functions.Functions;
using AzureFunctionsTime.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace AzureFunctionsTime.Tests.Tests
{
    public class TimeApiTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateTime_Should_Return_200()
        {
            // Arrenge
            MockCloudTableTimes mockTodos = new MockCloudTableTimes(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time timeRequest = TestFactory.GetTimeRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeRequest);

            // Act
            IActionResult response = await TimeApi.CreateTime(request, mockTodos, logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void UpdateTime_Should_Return_200()
        {
            // Arrenge
            MockCloudTableTimes mockTodos = new MockCloudTableTimes(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time timeRequest = TestFactory.GetTimeRequest();
            Guid timeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId, timeRequest);

            // Act
            IActionResult response = await TimeApi.UpdateTime(request, mockTodos, timeId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }


        [Fact]
        public async void GetAllTime_Should_Return_200()
        {
            //Arrenge
            MockCloudTableTimes mockTimes = new MockCloudTableTimes(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            //Time timeRequest = TestFactory.GetTimeRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest();

            //Act
            IActionResult response = await TimeApi.GetAllTimes(request, mockTimes, logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }

        [Fact]
        public async void GetTimeById_Should_Return_200()
        {
            //Arrenge
            MockCloudTableTimes mockTimes = new MockCloudTableTimes(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time timeRequest = TestFactory.GetTimeRequest();
            Guid timeId = Guid.NewGuid();

            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId, timeRequest);

            //Act
            IActionResult response = await TimeApi.GetTimeById(request, mockTimes, timeId.ToString, logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }
    }
}


    }
}





