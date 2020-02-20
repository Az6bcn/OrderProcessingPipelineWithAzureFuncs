using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace OrderProcessingPipelineWithAzureFuncs
{
    public static class EmailLicenseFile
    {
        [FunctionName("EmailLicenseFile")]
        public static void Run([BlobTrigger("licenses/{name}", Connection = "AzureWebJobsStorage")]string licenseFileContents, string name, ILogger log,
            [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {licenseFileContents.Length} Bytes");

            var Email = Regex.Match(input: licenseFileContents, pattern: @"^Email\:\ (.+)$", RegexOptions.Multiline).Groups[1].Value;

            message = new SendGridMessage()
            {

            };
        }
    }
}
