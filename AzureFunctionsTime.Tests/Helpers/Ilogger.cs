using System;

namespace AzureFunctionsTime.Tests.Helpers
{
    public class Ilogger
    {
        public static implicit operator Ilogger(ListLogger v)
        {
            throw new NotImplementedException();
        }
    }
}