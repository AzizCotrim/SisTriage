using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisTriage.Application.DTOs.Common;
using SisTriage.Application.DTOs.Usuario;
using SisTriage.Application.Services.Interfaces;
using System.Security.Claims;

namespace SisTriage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Recepcionista")]
        public async Task<ActionResult<UsuarioResponse>> Criar(CriarUsuarioRequest request)
        {
            var response = await _service.CriarAsync(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var token = await _service.AutenticarAsync(request);
            return Ok(new { token });
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResultadoPaginado<UsuarioResponse>>> Listar([FromQuery] PaginacaoRequest paginacao)
        {
            var response = await _service.ListarAsync(paginacao);
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UsuarioResponse>> ObterUsuarioLogado()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim == null || !Guid.TryParse(idClaim, out var id))
                return Unauthorized();

            var response = await _service.ObterPorIdAsync(id);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UsuarioResponse>> Atualizar(Guid id, AtualizarUsuarioRequest request)
        {
            var response = await _service.AtualizarAsync(id, request);
            return Ok(response);
        }
    }
}
