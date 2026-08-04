using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisTriage.Application.DTOs.Triagem;
using SisTriage.Application.Services.Interfaces;

namespace SisTriage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TriagemController : ControllerBase
    {
        private readonly ITriagemService _service;

        public TriagemController(ITriagemService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<TriagemResponse>> Registrar(CriarTriagemRequest request)
        {
            var response = await _service.RegistrarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TriagemResponse>> ObterPorId(Guid id)
        {
            var response = await _service.ObterPorIdAsync(id);
            if (response == null)
                return NotFound();

            return Ok(response);
        }
    }
}
