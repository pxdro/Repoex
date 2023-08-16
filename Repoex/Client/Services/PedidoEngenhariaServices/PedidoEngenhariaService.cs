using System.Net.Http.Json;

namespace Repoex.Client.Services.PedidoEngenhariaServices
{
    public class PedidoEngenhariaService : IPedidoEngenhariaService
    {
        private readonly HttpClient _httpClient;

        public PedidoEngenhariaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PedidoEngenharia>> ObterPedidosEngenharia()
        {
            var result = await _httpClient.GetFromJsonAsync<List<PedidoEngenharia>>("api/pedidoengenharia");
            if (result != null)
            {
                return result;
            }
            return new List<PedidoEngenharia>();
        }
    }
}
