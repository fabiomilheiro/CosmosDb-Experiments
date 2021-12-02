using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace console
{
    class Program
    {
        public static readonly string _endpointUri = "https://localhost:8081";
        public static readonly string _primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        static async Task Main(string[] args)
        {
            await OrdersExample.Run();
        }
    }
}
