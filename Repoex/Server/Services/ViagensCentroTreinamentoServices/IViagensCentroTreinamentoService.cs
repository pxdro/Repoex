namespace Repoex.Server.Services.ViagensCentroTreinamentoServices
{
    public interface IViagensCentroTreinamentoService
    {
        Task<List<ViagensCentroTreinamento>> ObterRelatorio();
    }
}