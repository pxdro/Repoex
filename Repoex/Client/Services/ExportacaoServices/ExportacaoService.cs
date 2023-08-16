using System.Net.Http.Json;

namespace Repoex.Client.Services.ExportacaoServices
{
    public class ExportacaoService : IExportacaoService
    {
        private readonly HttpClient _httpClient;

        public ExportacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Exportacao>> ObterExportacoes()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Exportacao>>("api/exportacao");
            if (result != null)
            {
                return result;
            }
            return new List<Exportacao>();
        }
    }
}