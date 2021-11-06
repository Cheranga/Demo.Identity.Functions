using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Demo.Identity.PurchaseOrders.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.Identity.PurchaseOrders.Functions
{
    public class ProcessOrderFunction
    {
        private readonly ILogger<ProcessOrderFunction> _logger;

        public ProcessOrderFunction(ILogger<ProcessOrderFunction> logger)
        {
            _logger = logger;
        }

        [FunctionName(nameof(ProcessOrderFunction))]
        public async Task RunAsync([ServiceBusTrigger("%ServiceBusConfig:Topic%", "%ServiceBusConfig:Subscription%", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            var messageContent = Encoding.UTF8.GetString(message.Body);

            var purchaseOrder = JsonConvert.DeserializeObject<PurchaseOrder>(messageContent);
            
            _logger.LogInformation("Received purchase order: {@PurchaseOrder}", purchaseOrder);
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}