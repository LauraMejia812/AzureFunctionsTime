using System;

namespace AzureFunctionsTime.common.Models
{
    public class Time
    {
        public int EmployeeId { get; set; }
        public DateTime EntryDate { get; set; }
        public int Type { get; set; }
        public bool Consolidated { get; set; }
    }
}
