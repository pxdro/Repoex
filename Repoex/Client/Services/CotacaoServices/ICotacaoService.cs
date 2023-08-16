namespace Repoex.Client.Services.CotacaoServices
{
    public interface ICotacaoService
    {
        Task<List<Cotacao>> ObterCotacoes();

    }
}
