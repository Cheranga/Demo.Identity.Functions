using System.Threading.Tasks;
using Demo.Identity.PurchaseOrders.Infrastructure;
using Demo.Identity.PurchaseOrders.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Demo.Identity.PurchaseOrders.Services
{
    public interface IReceiveOrderRequestHandler
    {
        Task<bool> HandleAsync(PurchaseOrder order);

        Task<bool> HandleAsync(PurchaseOrder order, IAsyncCollector<PurchaseOrder> messages);
    }
    
    public class ReceiveOrderRequestHandler : IReceiveOrderRequestHandler
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<ReceiveOrderRequestHandler> _logger;

        public ReceiveOrderRequestHandler(IMessagePublisher messagePublisher, ILogger<ReceiveOrderRequestHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }
        
        public async Task<bool> HandleAsync(PurchaseOrder order)
        {
            var operation = await _messagePublisher.PublishAsync(order);
            return operation;
        }

        public async Task<bool> HandleAsync(PurchaseOrder order, IAsyncCollector<PurchaseOrder> messages)
        {
            await messages.AddAsync(order);
            return true;
        }
    }
}