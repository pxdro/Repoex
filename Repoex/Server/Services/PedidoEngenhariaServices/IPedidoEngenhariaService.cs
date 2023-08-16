namespace Repoex.Server.Services.PedidoEngenhariaServices
{
    public interface IPedidoEngenhariaService
    {
        Task<List<PedidoEngenharia>> ObterRelatorio();
    }
}
