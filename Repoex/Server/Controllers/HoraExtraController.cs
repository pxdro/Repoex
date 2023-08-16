using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Services.HoraExtraServices;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "HoraExtra")]
    [Route("api/[controller]")]
    [ApiController]
    public class HoraExtraController : Controller
    {
        private readonly IHoraExtraService _horaExtraService;

        public HoraExtraController(IHoraExtraService horaExtraService)
        {
            _horaExtraService = horaExtraService;
        }

        [HttpGet]
        public async Task<List<HoraExtra>> ObterRelatorios()
        {
            return await _horaExtraService.ObterRelatorio();
        }
    }
}