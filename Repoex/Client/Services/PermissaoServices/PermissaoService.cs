using System.Net.Http.Json;

namespace Repoex.Client.Services.PermissaoServices
{
    public class PermissaoService : IPermissaoService
    {
        private readonly HttpClient _httpClient;

        public PermissaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PermissaoVM>> ObterTodos()
        {
            var result = await _httpClient.GetFromJsonAsync<List<PermissaoVM>>("api/permissao");
            if (result != null)
            {
                return result;
            }
            throw new Exception("Nenhuma permissão encontrada.");
        }

        public async Task<PermissaoVM> ObterPorId(Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<PermissaoVM>($"api/permissao/{id}");
            if (result != null)
            {
                return result;
            }
            throw new Exception("Permissão não encontrada.");
        }

        public async Task<PermissaoVM> Adicionar(PermissaoVM permissaoVM)
        {
            var result = await _httpClient.PostAsJsonAsync("api/permissao", permissaoVM);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<PermissaoVM>();
                return response!;
            }
            else
                throw new Exception("Permissão já cadastrada no sistema.");

            throw new Exception("Erro ao criar a permissão.");
        }

        public async Task<PermissaoVM> Atualizar(PermissaoVM permissaoVM)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/permissao/{permissaoVM.Id}", permissaoVM);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<PermissaoVM>();
                return response!;
            }
            else
                throw new Exception("Permissão inserida já existente no sistema com outro ID.");

            throw new Exception("Erro ao atualizar a permissão.");
        }
    }
}
