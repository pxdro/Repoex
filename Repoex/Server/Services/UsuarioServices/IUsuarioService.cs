using Repoex.Shared.ViewModels;

namespace Repoex.Server.Services.UsuarioServices
{
    public interface IUsuarioService : IDisposable
    {
        Task<Usuario?> Logar(UsuarioDto usuario);

        Task<Usuario?> Adicionar(Usuario usuario);

        Task<Usuario?> Atualizar(Usuario usuario);

        string CreateToken(Usuario usuario);
    }
}
