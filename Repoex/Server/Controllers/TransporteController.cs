using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Services.TransporteServices;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "Transporte")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransporteController : Controller
    {
        private readonly ITransporteService _transporteService;

        public TransporteController(ITransporteService TransporteService)
        {
            _transporteService = TransporteService;
        }

        [HttpGet]
        public async Task<List<Transporte>> ObterRelatorios()
        {
            return await _transporteService.ObterRelatorio();
        }
    }
}