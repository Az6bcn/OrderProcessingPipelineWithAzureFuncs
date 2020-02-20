using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessingPipelineWithAzureFuncs.Models
{
    public class Order
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string Email { get; set; }
        public decimal Price { get; set; }
    }
}
