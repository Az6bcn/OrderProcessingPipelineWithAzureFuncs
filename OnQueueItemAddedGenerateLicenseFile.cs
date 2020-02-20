using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OrderProcessingPipelineWithAzureFuncs.Models;

namespace OrderProcessingPipelineWithAzureFuncs
{
    public static class OnQueueItemAddedGenerateLicenseFile
    {
        [FunctionName("GenerateLicenseFile")]
        public static void Run([QueueTrigger("orders", Connection = "AzureWebJobsStorage")] Order order, ILogger log,
            // blob output binding i.e writes to the blob container
            [Blob("licenses/{rand-guid}.lic")] TextWriter outputBlob)
        {
            // when message is added to the orders queue this function is triggered and generates a license file for the added item in the blob storage.
            log.LogInformation($"Generating License for order : {order.OrderId}");

            outputBlob.WriteLine($"OrderId: {order.OrderId}");
            outputBlob.WriteLine($"Email: {order.Email}");
            outputBlob.WriteLine($"ProductId: {order.ProductId}");
            outputBlob.WriteLine($"PurchasedDate: {DateTime.UtcNow}");

            log.LogInformation($"Generating License for order : {order.OrderId}");
        }
    }
}
