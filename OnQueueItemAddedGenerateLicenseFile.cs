using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OrderProcessingPipelineWithAzureFuncs.Models;

namespace OrderProcessingPipelineWithAzureFuncs
{
    public static class OnQueueItemAddedGenerateLicenseFile
    {
        [FunctionName("GenerateLicenseFile")]
        public static async Task Run([QueueTrigger("orders", Connection = "AzureWebJobsStorage")] Order order, ILogger log,
            // blob output binding i.e writes to the blob container
            //[Blob("licenses/{rand-guid}.lic")] TextWriter outputBlob,
            IBinder binder)
        {
            // when message is added to the orders queue this function is triggered and generates a license file for the added item in the blob storage.
            log.LogInformation($"Generating License for order : {order.OrderId}");

            // to the blob storage myself at run time by using IBinder so that I can access the content/item coming in.
            var blobAttribute = new BlobAttribute("licenses/{order.OrderId}.lic");
            var blobOutputBinding = await binder.BindAsync<TextWriter>(blobAttribute); // TextWriter: output binding property


            blobOutputBinding.WriteLine($"OrderId: {order.OrderId}");
            blobOutputBinding.WriteLine($"Email: {order.Email}");
            blobOutputBinding.WriteLine($"ProductId: {order.ProductId}");
            blobOutputBinding.WriteLine($"PurchasedDate: {DateTime.UtcNow}");

            log.LogInformation($"Generating License for order : {order.OrderId}");
        }
    }
}

/*
 * This queue is triggered when an iten is added to the queue from OnPaymentReceived.
 * We can bind to blob this queue writes to at runtime so that we have access to the content/item its gonna write into the blob
 * and do some manipulation e.g use the items orderId as part of the file name. 
 * By using the IBinder
 */
