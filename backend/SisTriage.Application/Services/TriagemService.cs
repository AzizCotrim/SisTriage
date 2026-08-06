using AutoMapper;
using SisTriage.Application.DTOs.Triagem;
using SisTriage.Application.Services.Interfaces;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;

namespace SisTriage.Application.Services
{
    public class TriagemService : ITriagemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TriagemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TriagemResponse> RegistrarAsync(CriarTriagemRequest request)
        {
            var paciente = await _unitOfWork.Pacientes.GetByIdAsync(request.PacienteId);
            if (paciente == null)
                throw new RecursoNaoEncontradoException("Paciente não encontrado.");

            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(request.UsuarioId);
            if (usuario == null)
                throw new RecursoNaoEncontradoException("Usuário não encontrado.");

            var triagem = _mapper.Map<Triagem>(request);
            triagem.DataHora = DateTime.UtcNow;
            triagem.Status = StatusTriagem.Aguardando;

            _unitOfWork.Triagens.Add(triagem);

            var filaAtendimento = new FilaAtendimento
            {
                TriagemId = triagem.Id,
                DataEntrada = DateTime.UtcNow,
                Status = StatusFila.Aguardando
            };

            _unitOfWork.Filas.Add(filaAtendimento);

            await _unitOfWork.SalvarAsync();

            var triagemCompleta = await _unitOfWork.Triagens.ObterComPacienteEUsuarioAsync(triagem.Id);

            return _mapper.Map<TriagemResponse>(triagemCompleta);
        }

        public async Task<TriagemResponse?> ObterPorIdAsync(Guid id)
        {
            var triagem = await _unitOfWork.Triagens.ObterComPacienteEUsuarioAsync(id);

            return triagem == null ? null : _mapper.Map<TriagemResponse>(triagem);
        }
    }
}
