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
        public static readonly string _endpointUri = "https://cosmoslab99236.documents.azure.com:443/";
        public static readonly string _primaryKey = "WShx23AHHgV0eetZezHJWBhJqnuTqF2RSmBTVVMYEHFwoh23nOVEQZRHYfDqInefalJwKzBiSoeSwD1W8oC1VQ==";

        static async Task Main(string[] args)
        {
            await OrdersExample.Run();
        }
    }
}
