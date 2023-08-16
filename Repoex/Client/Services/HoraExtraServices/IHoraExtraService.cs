namespace Repoex.Client.Services.HoraExtraServices
{
    public interface IHoraExtraService
    {
        Task<List<HoraExtra>> ObterHorasExtras();
    }
}