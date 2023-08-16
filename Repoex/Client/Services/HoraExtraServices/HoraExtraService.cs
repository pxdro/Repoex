using System.Net.Http.Json;

namespace Repoex.Client.Services.HoraExtraServices
{
    public class HoraExtraService : IHoraExtraService
    {
        private readonly HttpClient _httpClient;

        public HoraExtraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<HoraExtra>> ObterHorasExtras()
        {
            var result = await _httpClient.GetFromJsonAsync<List<HoraExtra>>("api/horaextra");
            if (result != null)
            {
                return result;
            }
            return new List<HoraExtra>();
        }
    }
}