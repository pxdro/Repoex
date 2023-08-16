namespace Repoex.Server.Services.HoraExtraServices
{
    public interface IHoraExtraService
    {
        Task<List<HoraExtra>> ObterRelatorio();
    }
}