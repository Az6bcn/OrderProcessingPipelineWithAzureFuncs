using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderProcessingPipelineWithAzureFuncs.Models;

namespace OrderProcessingPipelineWithAzureFuncs
{
    public static class OnPaymentReceived
    {
        [FunctionName("OnPaymentReceived")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log,
            // queue output binding i.e writes to the queue
            [Queue("orders", Connection = "AzureWebJobsStorage")] IAsyncCollector<Order> orderQueue,
            // another output binding, table storage output binding i.e writes to the table
            [Table("orders", Connection = "AzureWebJobsStorage")] IAsyncCollector<Order> orderTable)
        {
            log.LogInformation("Received a payment");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Order data = JsonConvert.DeserializeObject<Order>(requestBody);

            // write to queue
            await orderQueue.AddAsync(data);

            data.PartitionKey = "orders"; // just one partition key (for demo purposes)
            data.RowKey = data.OrderId;

            // add to table storage
            await orderTable.AddAsync(data);

            return new OkObjectResult("Thank you for your purchse");

        }
    }
}
