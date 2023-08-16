using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repoex.Server.Services.PedidoEngenhariaServices;

namespace Repoex.Server.Controllers
{
    [Authorize(Roles = "PedidoEngenharia")]
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoEngenhariaController : Controller
    {
        private readonly IPedidoEngenhariaService _pedidoEngenhariaService;

        public PedidoEngenhariaController(IPedidoEngenhariaService pedidoEngenhariaService)
        {
            _pedidoEngenhariaService = pedidoEngenhariaService;
        }

        [HttpGet]
        public async Task<List<PedidoEngenharia>> ObterPedidosEngenharia()
        {
            return await _pedidoEngenhariaService.ObterRelatorio();
        }
    }
}
