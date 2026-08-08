using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisTriage.Application.DTOs.Indicador;
using SisTriage.Application.Services.Interfaces;

namespace SisTriage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IndicadorController : ControllerBase
    {
        private readonly IIndicadorService _service;

        public IndicadorController(IIndicadorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IndicadoresResponse>> Obter([FromQuery] IndicadoresRequest request)
        {
            var response = await _service.ObterAsync(request);
            return Ok(response);
        }
    }
}
