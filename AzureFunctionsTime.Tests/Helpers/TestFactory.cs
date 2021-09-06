using AzureFunctionsTime.common.Models;
using AzureFunctionsTime.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AzureFunctionsTime.Tests.Helpers
{
    public class TestFactory
    {
        public static TimeEntity GetTimeEntity()
        {
            return new TimeEntity
            {
                ETag = "*",
                PartitionKey = "TIME",
                RowKey = Guid.NewGuid().ToString(),
                EmployeeId = 10,
                EntryDate = DateTime.UtcNow,
                Type = 0,
                Consolidated = false
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid timeId, Time timeRequest)
        {
            string request = JsonConvert.SerializeObject(timeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{timeId}"
            };
        }


        public static DefaultHttpRequest CreateHttpRequest(Guid timeId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{timeId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Time timeRequest)
        {
            string request = JsonConvert.SerializeObject(timeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        public static Time GetTimeRequest()
        {
            return new Time
            {
               EmployeeId=11,
               EntryDate = DateTime.UtcNow,
               Type=0,
               Consolidated=false
            };
        }

        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Ilogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            Ilogger logger;
            if(type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = (Ilogger)NullLoggerFactory.Instance.CreateLogger("Null Logger"); 
            }
            return logger;
        }
    }
}
