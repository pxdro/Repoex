namespace Repoex.Server.Services.DecolideiaServices
{
    public interface IDecolideiaService
    {
        Task<List<Decolideia>> ObterRelatorio();
    }
}