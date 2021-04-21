using System.Net.Http;
using System.Threading.Tasks;

namespace Pds.Contracts.Data.Api.Client.Tests.Unit.Dummy
{
    internal interface IDummyService
    {
        Task<HttpResponseMessage> MakeHttpCallAsync();
    }
}