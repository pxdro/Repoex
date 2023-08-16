namespace Repoex.Server.Services.TransporteServices
{
    public interface ITransporteService
    {
        Task<List<Transporte>> ObterRelatorio();
    }
}