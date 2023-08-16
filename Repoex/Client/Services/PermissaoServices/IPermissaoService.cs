namespace Repoex.Client.Services.PermissaoServices
{
    public interface IPermissaoService
    {
        Task<List<PermissaoVM>> ObterTodos();

        Task<PermissaoVM> ObterPorId(Guid id);

        Task<PermissaoVM> Adicionar(PermissaoVM permissaoVM);

        Task<PermissaoVM> Atualizar(PermissaoVM permissaoVM);
    }
}
