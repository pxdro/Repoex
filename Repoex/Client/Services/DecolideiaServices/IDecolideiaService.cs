namespace Repoex.Client.Services.DecolideiaServices
{
    public interface IDecolideiaService
    {
        Task<List<Decolideia>> ObterIdeias();
    }
}