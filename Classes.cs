using System;
using System.Collections.Generic;

namespace console
{
    public class Order
    {
        public string Id { get; set; }
        public int? TrackingNumber { get; set; }
        public string PartitionKey { get; set; }
        public StreetAddress ShippingAddress { get; set; }
    }

    public class StreetAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
    }

    public class Distributor
    {
        public string Id { get; set; }
        //public string ETag { get; set; }
        public ICollection<StreetAddress> ShippingCenters { get; set; }
    }
}