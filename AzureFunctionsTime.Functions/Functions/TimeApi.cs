using AzureFunctionsTime.common.Models;
using AzureFunctionsTime.common.Responses;
using AzureFunctionsTime.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureFunctionsTime.Functions.Functions
{
    public static class TimeApi
    {
        [FunctionName(nameof(CreateTime))]
        public static async Task<IActionResult> CreateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "time")] HttpRequest req,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            ILogger log)
        {
            log.LogInformation("Recieved a new register");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Time time = JsonConvert.DeserializeObject<Time>(requestBody);

            if (time == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Fields must be entered for employee registration."
                });
            }

            if (time != null && time.EmployeeId <= 0)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "to record the time, you have to correctly send the id of the employee."
                });
            }

            if (time.Type != 0 && time.Type != 1)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "to record the time,Type can only be 0 or 1."
                });
            }
            
                TimeEntity timeEntity = new TimeEntity
                {
                    EmployeeId = time.EmployeeId,
                    EntryDate = DateTime.UtcNow,
                    Type = time.Type,
                    ETag = "*",
                    Consolidated = false,
                    PartitionKey = "TIME",
                    RowKey = Guid.NewGuid().ToString(),

                };

                TableOperation addOperation = TableOperation.Insert(timeEntity);
                await timeTable.ExecuteAsync(addOperation);

                string message = "New register stored in table";
                log.LogInformation(message);

                return new OkObjectResult(new Response
                {
                    IsSuccess = true,
                    Message = message,
                    Result = timeEntity
                });
            }

        
        [FunctionName(nameof(UpdateTime))]
        public static async Task<IActionResult> UpdateTime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "time/{id}")] HttpRequest req,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            string id, 
            ILogger log)
        {
            log.LogInformation($"Update for time: {id}, received.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Time time = JsonConvert.DeserializeObject<Time>(requestBody);

            //Validate time by Id

            TableOperation findOperation = TableOperation.Retrieve<TimeEntity>("TIME", id);  
            TableResult findResult = await timeTable.ExecuteAsync(findOperation);

            if (findResult.Result == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Time not found."
                });
            }

            //Update time

            TimeEntity timeEntity = (TimeEntity)findResult.Result;
            timeEntity.Consolidated = time.Consolidated;
            if (time.Type != 0 && time.Type != 1)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "to record the time,Type can only be 0 or 1."
                });
            }

                        
            if (time.Type == timeEntity.Type)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "to update the time you must enter a different value in the type."
                });
                
            }
            else   
            {
                timeEntity.Type = time.Type;
            }


            TableOperation addOperation = TableOperation.Replace(timeEntity);
            await timeTable.ExecuteAsync(addOperation);

            string message = $"Time: {id}, update in table.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEntity
            });
        }

        [FunctionName(nameof(GetAllTimes))]
        public static async Task<IActionResult> GetAllTimes(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time")] HttpRequest req,
        [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
        ILogger log)
        {
            log.LogInformation("Get all times received.");

            TableQuery<TimeEntity> query = new TableQuery<TimeEntity>();
            TableQuerySegment<TimeEntity> times = await timeTable.ExecuteQuerySegmentedAsync(query, null);

            string message = "Retrieved all times.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = times
            });
        }

        [FunctionName(nameof(GetTimeById))]
        public static IActionResult GetTimeById(        
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time/{id}")] HttpRequest req,
        [Table("time", "TIME", "{id}", Connection = "AzureWebJobsStorage")] TimeEntity timeEntity,
        string id,
        ILogger log)
        {
            log.LogInformation($"Get time by id: {id}, received.");

            if (timeEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Time not found."
                });
            }

            string message = $"Time: {timeEntity.RowKey}, retrieved.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = timeEntity
            });
        }
    }
}

