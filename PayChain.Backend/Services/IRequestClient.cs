using System.Net.Http;
using System.Threading.Tasks;

namespace PayChain.Backend.Services
{
    public interface IRequestClient
    {
        Task<HttpResponseMessage> MakeRequest(HttpRequestMessage request);
    }
}
