using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisTriage.Application.DTOs.FilaAtendimento;
using SisTriage.Application.Services.Interfaces;

namespace SisTriage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilaAtendimentoController : ControllerBase
    {
        private readonly IFilaAtendimentoService _service;

        public FilaAtendimentoController(IFilaAtendimentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilaAtendimentoResponse>>> ObterFilaOrdenada()
        {
            var response = await _service.ObterFilaOrdenadaAsync();
            return Ok(response);
        }

        [HttpPost("chamar-proximo")]
        public async Task<ActionResult<FilaAtendimentoResponse>> ChamarProximo()
        {
            var response = await _service.ChamarProximoAsync();
            if (response == null)
                return NoContent();

            return Ok(response);
        }

        [HttpPost("{id}/finalizar")]
        public async Task<ActionResult<FilaAtendimentoResponse>> Finalizar(Guid id)
        {
            var response = await _service.FinalizarAsync(id);
            return Ok(response);
        }
    }
}
