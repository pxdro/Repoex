using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Repositories.UsuarioRepository;
using Repoex.Server.Services.UsuarioServices;
using Repoex.Shared.ViewModels;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioRepository usuarioRepository,
                                IUsuarioService usuarioService,
                                IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UsuarioVM>> ObterTodos()
        {
            var usuarios = (await _usuarioRepository.ObterTodosComPermissoes()).ToList();
            var usuariosVM = usuarios.Select(usu => _mapper.Map<UsuarioVM>(usu));
            return usuariosVM;
        }

        [HttpGet("{id:guid}")]
        public async Task<UsuarioVM?> ObterUsuario([FromRoute] Guid id)
        {
            var usuario = await _usuarioRepository.ObterUsuarioPorIdPermissoes(id);
            _usuarioRepository.Dispose();
            var usuarioVM = _mapper.Map<UsuarioVM>(usuario);
            return usuarioVM;
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioVM>> Adicionar([FromBody] UsuarioVM usuarioVM)
        {
            var usuario = _mapper.Map<Usuario>(usuarioVM);
            var permissoes = usuario.Permissoes;
            usuario.Permissoes = null;

            var usu = await _usuarioService.Adicionar(usuario);
            if (usu == null)
                return BadRequest(new { error = "Login já cadastrado no sistema" });

            usuario.Permissoes = permissoes;
            await _usuarioRepository.SalvarAlteracoes();

            return Ok(usuarioVM);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UsuarioVM>> Atualizar([FromBody] UsuarioVM usuarioVM, [FromRoute] Guid id)
        {
            var usuario = _mapper.Map<Usuario>(usuarioVM);
            var permissoes = usuario.Permissoes;
            usuario.Permissoes = null;

            if (usuarioVM.Id == id)
            {
                // Update Usuario
                var usu = await _usuarioService.Atualizar(usuario);
                if (usu == null)
                    return BadRequest(new { error = "Login informado já cadastrado no sistema com outro usuário" });
                _usuarioRepository.EncerrarTracker();

                // Update Permissoes - Excluir
                usuario = await _usuarioRepository.ObterUsuarioPorIdPermissoes(id);
                usuario!.Permissoes = null;
                await _usuarioRepository.SalvarAlteracoes();
                _usuarioRepository.EncerrarTracker();

                // Update Permissoes - Inserir
                usuario = await _usuarioRepository.ObterUsuarioPorIdPermissoes(id);
                usuario!.Permissoes = permissoes;
                await _usuarioRepository.SalvarAlteracoes();
                _usuarioRepository.EncerrarTracker();
            }
            else
                return BadRequest(new { error = "Os IDs informados são distintos ou um usuário com esse ID não existe" });

            return Ok(usuarioVM);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<UsuarioVM>> Excluir([FromRoute] Guid id)
        {
            var usuario = (await _usuarioRepository.Buscar(usu => usu.Id == id)).FirstOrDefault();

            if (usuario == null)
                return NotFound();

            await _usuarioRepository.Remover(id);

            return _mapper.Map<UsuarioVM>(usuario);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = await _usuarioService.Logar(usuarioDto);

            if (usuario == null)
            {
                return BadRequest(new { error = "Usuário não cadastrado no sistema, inativo ou senha incorreta." });
            }

            usuarioDto.Nome = usuario.Nome;
            usuarioDto.Senha = string.Empty;
            usuarioDto.Jwt = _usuarioService.CreateToken(usuario); ;

            return Ok(usuarioDto);
        }
    }
}
