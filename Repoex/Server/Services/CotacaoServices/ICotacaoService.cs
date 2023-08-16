namespace Repoex.Server.Services.CotacaoServices
{
    public interface ICotacaoService
    {
        Task<List<Cotacao>> ObterRelatorio();
    }
}
