namespace Repoex.Client.Services.UsuarioServices
{
    public interface IUsuarioService
    {
        Task<List<UsuarioVM>> ObterTodos();

        Task<UsuarioVM> ObterPorId(Guid id);

        Task<UsuarioVM> Adicionar(UsuarioVM usuarioVM);

        Task<UsuarioVM> Atualizar(UsuarioVM usuarioVM);

        Task<UsuarioVM> Remover(Guid id);

        Task<UsuarioDto> Login(UsuarioDto usuario);
    }
}
