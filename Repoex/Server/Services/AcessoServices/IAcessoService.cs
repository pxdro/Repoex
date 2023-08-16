namespace Repoex.Server.Services.AcessoServices
{
    public interface IAcessoService
    {
        Task<List<Acesso>> ObterRelatorio();
    }
}