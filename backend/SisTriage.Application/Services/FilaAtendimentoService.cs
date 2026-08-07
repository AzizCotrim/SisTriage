using AutoMapper;
using SisTriage.Application.DTOs.FilaAtendimento;
using SisTriage.Application.Services.Interfaces;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;

namespace SisTriage.Application.Services
{
    public class FilaAtendimentoService : IFilaAtendimentoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FilaAtendimentoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FilaAtendimentoResponse>> ObterFilaOrdenadaAsync()
        {
            var fila = await _unitOfWork.Filas.ObterFilaOrdenadaPorPrioridadeAsync();

            return _mapper.Map<IEnumerable<FilaAtendimentoResponse>>(fila);
        }

        public async Task<FilaAtendimentoResponse?> ChamarProximoAsync()
        {
            var fila = await _unitOfWork.Filas.ObterFilaOrdenadaPorPrioridadeAsync();

            var proximo = fila.FirstOrDefault();
            if (proximo == null)
                return null;

            proximo.Status = StatusFila.Chamado;
            proximo.Triagem.Status = StatusTriagem.EmAtendimento;

            await _unitOfWork.SalvarAsync();

            return _mapper.Map<FilaAtendimentoResponse>(proximo);
        }

        public async Task<FilaAtendimentoResponse> FinalizarAsync(Guid filaAtendimentoId)
        {
            var filaAtendimento = await _unitOfWork.Filas.ObterComTriagemEPacienteAsync(filaAtendimentoId);
            if (filaAtendimento == null)
                throw new RecursoNaoEncontradoException("Registro de fila não encontrado.");

            if (filaAtendimento.Status != StatusFila.Chamado)
                throw new OperacaoInvalidaException("Só é possível finalizar um atendimento que já foi chamado.");

            filaAtendimento.Status = StatusFila.Atendido;
            filaAtendimento.DataSaida = DateTime.UtcNow;
            filaAtendimento.Triagem.Status = StatusTriagem.Finalizada;

            await _unitOfWork.SalvarAsync();

            return _mapper.Map<FilaAtendimentoResponse>(filaAtendimento);
        }

        public async Task<FilaAtendimentoResponse> RegistrarNaoComparecimentoAsync(Guid filaAtendimentoId)
        {
            var filaAtendimento = await _unitOfWork.Filas.ObterComTriagemEPacienteAsync(filaAtendimentoId);
            if (filaAtendimento == null)
                throw new RecursoNaoEncontradoException("Registro de fila não encontrado.");

            if (filaAtendimento.Status != StatusFila.Chamado)
                throw new OperacaoInvalidaException("Só é possível registrar não comparecimento de um paciente que já foi chamado.");

            filaAtendimento.Status = StatusFila.NaoCompareceu;
            filaAtendimento.DataSaida = DateTime.UtcNow;
            filaAtendimento.Triagem.Status = StatusTriagem.Cancelada;

            await _unitOfWork.SalvarAsync();

            return _mapper.Map<FilaAtendimentoResponse>(filaAtendimento);
        }

        public async Task<IEnumerable<FilaAtendimentoResponse>> ObterNaoAtendidosAsync()
        {
            var naoAtendidos = await _unitOfWork.Filas.ObterPorStatusAsync(StatusFila.NaoCompareceu);

            return _mapper.Map<IEnumerable<FilaAtendimentoResponse>>(naoAtendidos);
        }
    }
}
