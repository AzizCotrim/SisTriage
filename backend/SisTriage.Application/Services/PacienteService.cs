using AutoMapper;
using SisTriage.Application.DTOs.Common;
using SisTriage.Application.DTOs.Paciente;
using SisTriage.Application.Services.Interfaces;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;

namespace SisTriage.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUsuarioLogado _usuarioLogado;

        public PacienteService(IUnitOfWork unitOfWork, IMapper mapper, IUsuarioLogado usuarioLogado)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _usuarioLogado = usuarioLogado;
        }

        public async Task<PacienteResponse> CriarAsync(CriarPacienteRequest request)
        {
            var pacienteExistente = await _unitOfWork.Pacientes.ObterPorCpfAsync(request.CPF);
            if (pacienteExistente != null)
                throw new RegistroDuplicadoException("Já existe um paciente cadastrado com esse CPF.");

            var paciente = _mapper.Map<Paciente>(request);

            _unitOfWork.Pacientes.Add(paciente);
            await _unitOfWork.SalvarAsync();

            return _mapper.Map<PacienteResponse>(paciente);
        }

        public async Task<PacienteResponse?> ObterPorIdAsync(Guid id)
        {
            var paciente = await _unitOfWork.Pacientes.GetByIdAsync(id);

            if (paciente == null)
                return null;

            // O interceptor cobre apenas escritas. A consulta ao prontuário completo
            // (CPF e alergias) é acesso a dado sensível e precisa ser registrada (LGPD).
            _unitOfWork.LogsAuditoria.Add(new LogAuditoria
            {
                UsuarioId = _usuarioLogado.Id,
                UsuarioEmail = _usuarioLogado.Email,
                Acao = AcaoAuditoria.Consulta,
                Entidade = nameof(Paciente),
                RegistroId = paciente.Id.ToString(),
                DataHora = DateTime.UtcNow
            });

            await _unitOfWork.SalvarAsync();

            return _mapper.Map<PacienteResponse>(paciente);
        }

        public async Task<ResultadoPaginado<PacienteListaResponse>> ListarAsync(PaginacaoRequest paginacao)
        {
            var (itens, total) = await _unitOfWork.Pacientes.ObterPaginadoAsync(paginacao.Pagina, paginacao.TamanhoPagina);

            return new ResultadoPaginado<PacienteListaResponse>
            {
                Itens = _mapper.Map<IEnumerable<PacienteListaResponse>>(itens),
                Pagina = paginacao.Pagina,
                TamanhoPagina = paginacao.TamanhoPagina,
                TotalRegistros = total
            };
        }

        public async Task<PacienteResponse> AtualizarAsync(Guid id, AtualizarPacienteRequest request)
        {
            var paciente = await _unitOfWork.Pacientes.GetByIdAsync(id);
            if (paciente == null)
                throw new RecursoNaoEncontradoException("Paciente não encontrado.");

            paciente.Nome = request.Nome;
            paciente.Sexo = request.Sexo;
            paciente.Telefone = request.Telefone;
            paciente.Alergias = request.Alergias;

            await _unitOfWork.SalvarAsync();

            return _mapper.Map<PacienteResponse>(paciente);
        }
    }
}