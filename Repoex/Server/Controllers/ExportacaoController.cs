using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Services.ExportacaoServices;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "Exportacao")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExportacaoController : Controller
    {
        private readonly IExportacaoService _exportacaoService;

        public ExportacaoController(IExportacaoService exportacaoService)
        {
            _exportacaoService = exportacaoService;
        }

        [HttpGet]
        public async Task<List<Exportacao>> ObterRelatorios()
        {
            return await _exportacaoService.ObterRelatorio();
        }
    }
}