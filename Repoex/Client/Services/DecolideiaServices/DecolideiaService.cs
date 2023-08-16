using System.Net.Http.Json;

namespace Repoex.Client.Services.DecolideiaServices
{
    public class DecolideiaService : IDecolideiaService
    {
        private readonly HttpClient _httpClient;

        public DecolideiaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Decolideia>> ObterIdeias()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Decolideia>>("api/decolideia");
            if (result != null)
            {
                return result;
            }
            return new List<Decolideia>();
        }
    }
}