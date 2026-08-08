using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisTriage.Application.DTOs.Common;
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
        [Authorize(Roles = "Enfermeiro,Medico")]
        public async Task<ActionResult<FilaAtendimentoResponse>> ChamarProximo()
        {
            var response = await _service.ChamarProximoAsync();
            if (response == null)
                return NoContent();

            return Ok(response);
        }

        [HttpPost("{id}/finalizar")]
        [Authorize(Roles = "Medico")]
        public async Task<ActionResult<FilaAtendimentoResponse>> Finalizar(Guid id)
        {
            var response = await _service.FinalizarAsync(id);
            return Ok(response);
        }

        [HttpPost("{id}/nao-compareceu")]
        [Authorize(Roles = "Enfermeiro,Medico")]
        public async Task<ActionResult<FilaAtendimentoResponse>> RegistrarNaoComparecimento(Guid id)
        {
            var response = await _service.RegistrarNaoComparecimentoAsync(id);
            return Ok(response);
        }

        [HttpGet("nao-atendidos")]
        public async Task<ActionResult<ResultadoPaginado<FilaAtendimentoResponse>>> ObterNaoAtendidos([FromQuery] PaginacaoRequest paginacao)
        {
            var response = await _service.ObterNaoAtendidosAsync(paginacao);
            return Ok(response);
        }
    }
}
