namespace Repoex.Client.Services.TransporteServices
{
    public interface ITransporteService
    {
        Task<List<Transporte>> ObterTransportes();
    }
}
