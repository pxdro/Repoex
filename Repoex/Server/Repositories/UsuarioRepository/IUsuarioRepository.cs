using Repoex.Server.Repositories.Repository;

namespace Repoex.Server.Repositories.UsuarioRepository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> ObterUsuarioPorIdPermissoes(Guid id);

        Task<IEnumerable<Usuario>> ObterTodosComPermissoes();
    }
}
