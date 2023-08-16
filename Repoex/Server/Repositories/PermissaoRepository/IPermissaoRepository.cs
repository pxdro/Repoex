using Repoex.Server.Repositories.Repository;

namespace Repoex.Server.Repositories.PermissaoRepository
{
    public interface IPermissaoRepository : IRepository<Permissao>
    {
        Task<Permissao?> ObterPermissaoPorIdUsuario(Guid id);
    }
}
