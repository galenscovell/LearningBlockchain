using System.Net.Http;
using System.Threading.Tasks;

namespace PayChain.Backend.Services
{
    public class RequestClient : IRequestClient
    {
        private readonly HttpClient _client;

        public RequestClient()
        {
            _client = new HttpClient();
        }

        public async Task<HttpResponseMessage> MakeRequest(HttpRequestMessage request)
        {
            return await _client.SendAsync(request);
        }
    }
}
