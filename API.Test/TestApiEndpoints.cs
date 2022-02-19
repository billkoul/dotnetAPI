using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace API.Test
{
    public class TestApiEndpoints
    {
        private readonly string _apiBaseUrl = "http://localhost:50555";

        private readonly HttpClient _httpClient;

        public TestApiEndpoints()
        {
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task GetUploadsEndpoint()
        {
            _httpClient.DefaultRequestHeaders.Add("authorization", "apikeytest");

            var request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + "/api/Uploads/-1");
            var response = await _httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
