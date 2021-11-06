using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

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
        public async Task RunAsync([ServiceBusTrigger("%ServiceBusConfig:Topic%", "%ServiceBusConfig:Subscription%", Connection = "ServiceBusConnection")] string message)
        {
            _logger.LogInformation($"Input message: {message}");
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}