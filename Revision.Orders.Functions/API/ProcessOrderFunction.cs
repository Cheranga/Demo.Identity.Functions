using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Revision.Orders.Functions.API
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
            
            _logger.LogInformation($"Input message: {messageContent}");
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}