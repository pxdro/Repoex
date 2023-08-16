namespace Repoex.Client.Services.PedidoEngenhariaServices
{
    public interface IPedidoEngenhariaService
    {
        Task<List<PedidoEngenharia>> ObterPedidosEngenharia();
    }
}
