using AutoMapper;
using SisTriage.Application.DTOs.FilaAtendimento;
using SisTriage.Application.Services.Interfaces;
using SisTriage.Domain.Enums;
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
    }
}
