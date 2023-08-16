namespace Repoex.Server.Services.ExportacaoServices
{
    public interface IExportacaoService
    {
        Task<List<Exportacao>> ObterRelatorio();
    }
}