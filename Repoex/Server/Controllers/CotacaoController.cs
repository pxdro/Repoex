using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Services.CotacaoServices;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "Cotacao")]
    [Route("api/[controller]")]
    [ApiController]
    public class CotacaoController : Controller
    {
        private readonly ICotacaoService _cotacaoService;

        public CotacaoController(ICotacaoService cotacaoService)
        {
            _cotacaoService = cotacaoService;
        }

        [HttpGet]
        public async Task<List<Cotacao>> ObterRelatorios()
        {
            return await _cotacaoService.ObterRelatorio();
        }
    }
}
