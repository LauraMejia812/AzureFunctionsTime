using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using AzureFunctionsTime.common.Models;

namespace AzureFunctionsTime.Functions.Functions
{
    public static class ConsolidatedApi
    {

        [FunctionName(nameof(CreateConsolidated))]
        public static async Task<IActionResult> CreateConsolidated(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "consolidated")] HttpRequest req,
         [Table("consolidated", Connection = "AzureWebJobsStorage")] CloudTable consolidatedTable,
         ILogger log)
        {
            log.LogInformation("Recieved a new register");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Consolidated consolidated = JsonConvert.DeserializeObject<Consolidated>(requestBody);

            double minuteswork = 0;

            if (EmployeedId)









            if (time == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Fields must be entered for employee registration."
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
