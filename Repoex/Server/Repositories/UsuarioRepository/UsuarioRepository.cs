using Repoex.Server.Context;
using Repoex.Server.Repositories.Repository;

namespace Repoex.Server.Repositories.UsuarioRepository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(RepoexContext context) : base(context) { }

        public async Task<Usuario?> ObterUsuarioPorIdPermissoes(Guid id)
        {
            return await Db.Usuarios
                    .Include(usuario => usuario.Permissoes)
                    .FirstOrDefaultAsync(usuario => usuario.Id == id);
        }

        public async Task<IEnumerable<Usuario>> ObterTodosComPermissoes()
        {
            return await Db.Usuarios
                    .Include(usuario => usuario.Permissoes)
                    .AsNoTracking()
                    .ToListAsync();
        }
    }
}
