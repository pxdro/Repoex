using System.Net.Http.Json;

namespace Repoex.Client.Services.CotacaoServices
{
    public class CotacaoService : ICotacaoService
    {
        private readonly HttpClient _httpClient;

        public CotacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Cotacao>> ObterCotacoes()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Cotacao>>("api/cotacao");
            if (result != null)
            {
                return result;
            }
            return new List<Cotacao>();
        }
    }
}
