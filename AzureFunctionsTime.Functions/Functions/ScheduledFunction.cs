using AzureFunctionsTime.Functions.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace AzureFunctionsTime.Functions.Functions
{
    public static class ScheduledFunction
    {
        [FunctionName("ScheduledFunction")]
        public static async Task Run(
            [TimerTrigger("0 */2 * * * *")] TimerInfo myTimer,
            [Table("time", Connection = "AzureWebJobsStorage")] CloudTable timeTable,
            ILogger log)
        {
            log.LogInformation($"Deleting completed function executed at: {DateTime.Now}");

            string filter = TableQuery.GenerateFilterConditionForBool("Consolidated", QueryComparisons.Equal, true);
            TableQuery<TimeEntity> query = new TableQuery<TimeEntity>().Where(filter);
            TableQuerySegment<TimeEntity> completedTimes = await timeTable.ExecuteQuerySegmentedAsync(query, null);
            int deleted = 0;
            foreach (TimeEntity completedTime in completedTimes)
            {
                await timeTable.ExecuteAsync(TableOperation.Delete(completedTime));
                deleted++;
            }

            log.LogInformation($"Deleted: {deleted} items at: {DateTime.Now}");
        }
    }
}
