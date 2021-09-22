using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Revision.Customers.Functions.API
{
    public class CreateCustomerFunction
    {
        [FunctionName(nameof(CreateCustomerFunction))]
        public async Task<IActionResult> CreateCustomerAsync([HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethod.Get), Route = "v1/customers")]
            HttpRequestMessage request)
        {
            return new OkResult();
        }
    }
}