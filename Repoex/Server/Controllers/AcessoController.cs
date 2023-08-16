using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Services.AcessoServices;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "Acesso")]
    [Route("api/[controller]")]
    [ApiController]
    public class AcessoController : Controller
    {
        private readonly IAcessoService _acessoService;

        public AcessoController(IAcessoService acessoService)
        {
            _acessoService = acessoService;
        }

        [HttpGet]
        public async Task<List<Acesso>> ObterRelatorios()
        {
            return await _acessoService.ObterRelatorio();
        }
    }
}