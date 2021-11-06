using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Revision.Orders.Functions.API
{
    public class ReceiveOrderFunction
    {
        [FunctionName(nameof(ReceiveOrderFunction))]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, WebRequestMethods.Http.Post, Route = "v1/orders")] HttpRequest request)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            return new OkResult();
        }
    }
}