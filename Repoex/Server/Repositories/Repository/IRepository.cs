using System.Linq.Expressions;

namespace Repoex.Server.Repositories.Repository
{
    public interface IRepository<TEntity> : IDisposable
    {
        Task Adicionar(TEntity entity);

        Task<TEntity?> ObterPorId(Guid id);

        Task<List<TEntity>> ObterTodos();

        Task Atualizar(TEntity entity);

        Task Remover(Guid id);

        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);

        Task<int> SalvarAlteracoes();

        void EncerrarTracker();
    }
}
