using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Revision.Orders.Functions.Extensions;
using Revision.Orders.Functions.Models;
using Revision.Orders.Functions.Services;

namespace Revision.Orders.Functions.API
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