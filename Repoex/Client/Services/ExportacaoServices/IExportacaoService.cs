namespace Repoex.Client.Services.ExportacaoServices
{
    public interface IExportacaoService
    {
        Task<List<Exportacao>> ObterExportacoes();
    }
}