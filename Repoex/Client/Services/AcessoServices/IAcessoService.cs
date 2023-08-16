namespace Repoex.Client.Services.AcessoServices
{
    public interface IAcessoService
    {
        Task<List<Acesso>> ObterAcessos();
    }
}
