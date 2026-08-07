using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisTriage.Application.DTOs.Paciente;
using SisTriage.Application.Services.Interfaces;

namespace SisTriage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _service;

        public PacienteController(IPacienteService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Recepcionista,Enfermeiro")]
        public async Task<ActionResult<PacienteResponse>> Criar(CriarPacienteRequest request)
        {
            var response = await _service.CriarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = response.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteResponse>> ObterPorId(Guid id)
        {
            var response = await _service.ObterPorIdAsync(id);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteListaResponse>>> Listar()
        {
            var response = await _service.ListarAsync();
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Recepcionista,Enfermeiro")]
        public async Task<ActionResult<PacienteResponse>> Atualizar(Guid id, AtualizarPacienteRequest request)
        {
            var response = await _service.AtualizarAsync(id, request);
            return Ok(response);
        }
    }
}
