using System;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Revision.Orders.Functions.Configs;
using Revision.Orders.Functions.Models;

namespace Revision.Orders.Functions.Infrastructure
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusConfig _serviceBusConfig;
        private readonly ILogger<MessagePublisher> _logger;

        public MessagePublisher(ServiceBusClient serviceBusClient, ServiceBusConfig serviceBusConfig, ILogger<MessagePublisher> logger)
        {
            _serviceBusClient = serviceBusClient;
            _serviceBusConfig = serviceBusConfig;
            _logger = logger;
        }
        
        public async Task<bool> PublishAsync<TMessage>(TMessage message) where TMessage : class
        {
            var messageToPublish = new ServiceBusMessage(JsonSerializer.SerializeToUtf8Bytes(message))
            {
                Subject = nameof(PurchaseOrder),
                ContentType = MediaTypeNames.Application.Json,
                CorrelationId = Guid.NewGuid().ToString("N")
            };

            var sender = _serviceBusClient.CreateSender(_serviceBusConfig.Topic);

            await sender.SendMessageAsync(messageToPublish);

            return true;
        }
    }
}