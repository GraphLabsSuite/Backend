using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GraphLabs.Backend.Api.Controllers
{
    internal static class HttpRequestExtensions
    {
        public static async Task<string> GetBodyAsString(this HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}