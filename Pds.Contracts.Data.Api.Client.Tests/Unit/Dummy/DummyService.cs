using System.Net.Http;
using System.Threading.Tasks;

namespace Pds.Contracts.Data.Api.Client.Tests.Unit.Dummy
{
    internal class DummyService : IDummyService
    {
        private readonly HttpClient _client;

        public DummyService(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> MakeHttpCallAsync()
        {
            return await _client.GetAsync("https://testhost/test/Get");
        }
    }
}