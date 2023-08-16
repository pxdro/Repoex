using System.Net.Http.Json;

namespace Repoex.Client.Services.UsuarioServices
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient _httpClient;

        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UsuarioVM>> ObterTodos()
        {
            var result = await _httpClient.GetFromJsonAsync<List<UsuarioVM>>("api/usuario");
            if (result != null)
            {
                return result;
            }
            throw new Exception("Nenhum usuário encontrado.");
        }

        public async Task<UsuarioVM> ObterPorId(Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<UsuarioVM>($"api/usuario/{id}");
            if (result != null)
            {
                return result;
            }
            throw new Exception("Usuário não encontrado.");
        }

        public async Task<UsuarioVM> Adicionar(UsuarioVM usuarioVM)
        {
            var result = await _httpClient.PostAsJsonAsync("api/usuario", usuarioVM);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<UsuarioVM>();
                return response!;
            }
            else
                throw new Exception("Login já cadastrado no sistema.");

            throw new Exception("Erro ao criar o usuário.");
        }


        public async Task<UsuarioVM> Atualizar(UsuarioVM usuarioVM)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/usuario/{usuarioVM.Id}", usuarioVM);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<UsuarioVM>();
                return response!;
            }
            else
                throw new Exception("Login inserido já cadastrado no sistema com outro usuário.");

            throw new Exception("Erro ao atualizar o usuário.");
        }

        public async Task<UsuarioVM> Remover(Guid id)
        {
            var result = await _httpClient.DeleteAsync($"api/usuario/{id}");
            var response = await result.Content.ReadFromJsonAsync<UsuarioVM>();
            if (response != null)
            {
                return response;
            }
            throw new Exception("Erro ao excluir o usuário.");
        }

        public async Task<UsuarioDto> Login(UsuarioDto usuario)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/usuario/login", usuario);

            var response = await result.Content.ReadFromJsonAsync<UsuarioDto>();

            if (result.IsSuccessStatusCode)
                return response!;
            else if (response != null)
                throw new Exception(response?.Error);

            throw new Exception("Falha no Login.");
        }
    }
}
