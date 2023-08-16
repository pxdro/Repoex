namespace Repoex.Client.Services.ViagensCentroTreinamentoServices
{
    public interface IViagensCentroTreinamentoService
    {
        Task<List<ViagensCentroTreinamento>> ObterViagensCentroTreinamento();
    }
}
