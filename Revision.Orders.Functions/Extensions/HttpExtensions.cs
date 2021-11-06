using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Revision.Orders.Functions.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<TModel> ToModel<TModel>(this HttpRequest request) where TModel : class, new()
        {
            try
            {
                var content = await new StreamReader(request.Body).ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new TModel();
                }

                var model = JsonConvert.DeserializeObject<TModel>(content);
                return model;
            }
            catch
            {
                return new TModel();
            }
        }
    }
}