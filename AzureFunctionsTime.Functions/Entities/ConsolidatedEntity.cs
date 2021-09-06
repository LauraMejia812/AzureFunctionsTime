using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionsTime.Functions.Entities
{
    public class ConsolidatedEntity : TableEntity
    {
        public int EmployeeId { get; set; }
        public DateTime EntryDate { get; set; }
        public double minutesWork { get; set; }

    }
}
