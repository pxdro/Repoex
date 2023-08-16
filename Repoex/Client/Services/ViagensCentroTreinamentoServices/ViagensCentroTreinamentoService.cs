using System.Net.Http.Json;

namespace Repoex.Client.Services.ViagensCentroTreinamentoServices
{
    public class ViagensCentroTreinamentoServices : IViagensCentroTreinamentoService
    {
        private readonly HttpClient _httpClient;

        public ViagensCentroTreinamentoServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ViagensCentroTreinamento>> ObterViagensCentroTreinamento()
        {
            var result = await _httpClient.GetFromJsonAsync<List<ViagensCentroTreinamento>>("api/viagenscentrotreinamento");
            if (result != null)
            {
                return result;
            }
            return new List<ViagensCentroTreinamento>();
        }
    }
}
