using Repoex.Server.Context;
using Repoex.Server.Repositories.Repository;

namespace Repoex.Server.Repositories.PermissaoRepository
{
    public class PermissaoRepository : Repository<Permissao>, IPermissaoRepository
    {
        public PermissaoRepository(RepoexContext context) : base(context) { }

        public async Task<Permissao?> ObterPermissaoPorIdUsuario(Guid id)
        {
            return await Db.Permissoes
                    .Include(permissao => permissao.Usuarios)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(permissao => permissao.Id == id);
        }
    }
}
