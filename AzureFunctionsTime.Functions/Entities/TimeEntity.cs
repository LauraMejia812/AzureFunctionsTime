using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace AzureFunctionsTime.Functions.Entities
{
    public class TimeEntity : TableEntity
    {
        public int EmployeeId { get; set; }
        public DateTime EntryDate { get; set; }
        public int Type { get; set; }
        public bool Consolidated { get; set; }
    }
}
