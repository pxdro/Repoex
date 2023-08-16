using System.Net.Http.Json;

namespace Repoex.Client.Services.AcessoServices
{
    public class AcessoService : IAcessoService
    {
        private readonly HttpClient _httpClient;

        public AcessoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Acesso>> ObterAcessos()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Acesso>>("api/acesso");
            if (result != null)
            {
                return result;
            }
            return new List<Acesso>();
        }
    }
}
