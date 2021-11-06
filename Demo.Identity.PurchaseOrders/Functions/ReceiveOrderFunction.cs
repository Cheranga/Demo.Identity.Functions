using System.Net;
using System.Threading.Tasks;
using Demo.Identity.PurchaseOrders.Extensions;
using Demo.Identity.PurchaseOrders.Models;
using Demo.Identity.PurchaseOrders.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Demo.Identity.PurchaseOrders.Functions
{
    public class ReceiveOrderFunction
    {
        private readonly IReceiveOrderRequestHandler _requestHandler;

        public ReceiveOrderFunction(IReceiveOrderRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }
        
        [FunctionName(nameof(ReceiveOrderFunction))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, WebRequestMethods.Http.Post, Route = "v1/orders")] HttpRequest request)
        {
            var purchaseOrder = await request.ToModel<PurchaseOrder>();
            await _requestHandler.HandleAsync(purchaseOrder);

            return new OkResult();
        }
    }
}