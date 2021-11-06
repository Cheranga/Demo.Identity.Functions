using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Revision.Orders.Functions.Infrastructure;
using Revision.Orders.Functions.Models;

namespace Revision.Orders.Functions.Services
{
    public interface IReceiveOrderRequestHandler
    {
        Task<bool> HandleAsync(PurchaseOrder order);
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
    }
}