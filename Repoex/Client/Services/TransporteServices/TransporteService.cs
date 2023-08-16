using Repoex.Client.Services.TransporteServices;
using System.Net.Http.Json;

namespace Repoex.Client.Services.TrasporteServices
{
    public class TransporteServices : ITransporteService
    {
        private readonly HttpClient _httpClient;

        public TransporteServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Transporte>> ObterTransportes()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Transporte>>("api/transporte");
            if (result != null)
            {
                return result;
            }
            return new List<Transporte>();
        }
    }
}
