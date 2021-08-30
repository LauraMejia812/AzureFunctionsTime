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

        }
    }

