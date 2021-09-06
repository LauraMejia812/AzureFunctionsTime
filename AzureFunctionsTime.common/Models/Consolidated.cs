using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionsTime.common.Models
{
    public class Consolidated
    {
        public int EmployeeId { get; set; }
        public DateTime EntryDate { get; set; }
        public double minutesWork { get; set; }
        
    }
}
