using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Services.ViagensCentroTreinamentoServices;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "ViagensCentroTreinamento")]
    [Route("api/[controller]")]
    [ApiController]
    public class ViagensCentroTreinamentoController : Controller
    {
        private readonly IViagensCentroTreinamentoService _viagensCentroTreinamentoService;

        public ViagensCentroTreinamentoController(IViagensCentroTreinamentoService viagensCentroTreinamentoService)
        {
            _viagensCentroTreinamentoService = viagensCentroTreinamentoService;
        }

        [HttpGet]
        public async Task<List<ViagensCentroTreinamento>> ObterRelatorios()
        {
            return await _viagensCentroTreinamentoService.ObterRelatorio();
        }
    }
}