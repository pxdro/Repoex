using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Services.DecolideiaServices;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "Decolideia")]
    [Route("api/[controller]")]
    [ApiController]
    public class DecolideiaController : Controller
    {
        private readonly IDecolideiaService _decolideiaService;

        public DecolideiaController(IDecolideiaService decolideiaService)
        {
            _decolideiaService = decolideiaService;
        }

        [HttpGet]
        public async Task<List<Decolideia>> ObterRelatorios()
        {
            return await _decolideiaService.ObterRelatorio();
        }
    }
}