using System.Net;
using System.Threading.Tasks;
using Demo.Identity.PurchaseOrders.Extensions;
using Demo.Identity.PurchaseOrders.Models;
using Demo.Identity.PurchaseOrders.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace Demo.Identity.PurchaseOrders.Functions
{
    public class ReceiveOrderOutputBindingFunction
    {
        private readonly IReceiveOrderRequestHandler _requestHandler;

        public ReceiveOrderOutputBindingFunction(IReceiveOrderRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }
        
        [FunctionName(nameof(ReceiveOrderOutputBindingFunction))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, WebRequestMethods.Http.Post, Route = "v1/bad/orders")] HttpRequest request,
            [ServiceBus("%ServiceBusConfig:Topic%", ServiceBusEntityType.Topic, Connection = "ServiceBusConnection")]IAsyncCollector<PurchaseOrder> messages)
        {
            var purchaseOrder = await request.ToModel<PurchaseOrder>();
            await _requestHandler.HandleAsync(purchaseOrder, messages);

            return new OkResult();
        }
    }
}

