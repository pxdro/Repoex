using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Repositories.PermissaoRepository;
using Repoex.Shared.ViewModels;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissaoController : ControllerBase
    {
        private readonly IPermissaoRepository _permissaoRepository;
        private readonly IMapper _mapper;

        public PermissaoController(IPermissaoRepository permissaoRepository,
                                IMapper mapper)
        {
            _permissaoRepository = permissaoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<PermissaoVM>> ObterTodos()
        {
            var permissoes = (await _permissaoRepository.ObterTodos()).ToList();
            var permissoesVM = permissoes.Select(perm => _mapper.Map<PermissaoVM>(perm));
            return permissoesVM;
        }

        [HttpGet("{id:guid}")]
        public async Task<Permissao?> ObterPermissao([FromRoute] Guid id)
        {
            return await _permissaoRepository.ObterPorId(id);
        }

        [HttpPost]
        public async Task<ActionResult<Permissao>> Adicionar([FromBody] Permissao permissao)
        {
            var permissoes = await _permissaoRepository.ObterTodos();

            foreach (var perm in permissoes)
            {
                if (perm.Relatorio == permissao.Relatorio)
                    return BadRequest(new { error = "Permissão já cadastrada no sistema" });
            }

            await _permissaoRepository.Adicionar(permissao);

            return Ok(permissao);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Permissao>> Atualizar([FromBody] Permissao permissao, [FromRoute] Guid id)
        {
            if (permissao.Id == id)
            {
                var permissoes = await _permissaoRepository.ObterTodos();

                foreach (var perm in permissoes)
                {
                    if (perm.Relatorio == permissao.Relatorio && perm.Id != permissao.Id)
                        return BadRequest(new { error = "Permissão já cadastrada no sistema com outra ID" });
                }

                await _permissaoRepository.Atualizar(permissao);
            }
            else
                return BadRequest(new { error = "Os IDs informados são distintos ou uma permissão com esse ID não existe" });

            return Ok(permissao);
        }
    }
}
