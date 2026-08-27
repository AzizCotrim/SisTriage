using SisTriage.Application.DTOs.Indicador;
using SisTriage.Application.Services.Interfaces;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;

namespace SisTriage.Application.Services
{
    public class IndicadorService : IIndicadorService
    {
        private const int DiasPadrao = 30;

        private readonly IUnitOfWork _unitOfWork;

        public IndicadorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IndicadoresResponse> ObterAsync(IndicadoresRequest request)
        {
            // O Postgres exige DateTime em UTC. Datas vindas da query string chegam
            // com Kind=Unspecified, o que faria o Npgsql recusar a consulta.
            var inicio = ParaUtc(request.Inicio ?? DateTime.UtcNow.Date.AddDays(-DiasPadrao));
            var fim = ParaUtc(request.Fim ?? DateTime.UtcNow);

            if (inicio > fim)
                throw new OperacaoInvalidaException("A data inicial não pode ser maior que a data final.");

            var registros = (await _unitOfWork.Filas.ObterPorPeriodoAsync(inicio, fim)).ToList();

            var atendidos = registros
                .Where(f => f.Status == StatusFila.Atendido && f.DataSaida.HasValue)
                .ToList();

            var porClassificacao = atendidos
                .GroupBy(f => f.Triagem.Classificacao)
                .Select(grupo => new IndicadorPorClassificacaoResponse
                {
                    Classificacao = grupo.Key,
                    TempoMaximoEsperaMinutos = grupo.Key.TempoMaximoEsperaMinutos(),
                    Quantidade = grupo.Count(),
                    TempoMedioEsperaMinutos = Math.Round(grupo.Average(TempoEsperaMinutos), 1),
                    DentroDoTempo = grupo.Count(f => !ExcedeuTempo(f)),
                    ForaDoTempo = grupo.Count(ExcedeuTempo)
                })
                .OrderBy(indicador => indicador.Classificacao)
                .ToList();

            var dentroDoTempo = atendidos.Count(f => !ExcedeuTempo(f));
            var foraDoTempo = atendidos.Count(ExcedeuTempo);

            return new IndicadoresResponse
            {
                Inicio = inicio,
                Fim = fim,
                TotalRegistros = registros.Count,
                Atendidos = atendidos.Count,
                NaoComparecimentos = registros.Count(f => f.Status == StatusFila.NaoCompareceu),
                AindaNaFila = registros.Count(f => f.Status == StatusFila.Aguardando || f.Status == StatusFila.Chamado),
                TempoMedioEsperaMinutos = atendidos.Count == 0
                    ? 0
                    : Math.Round(atendidos.Average(TempoEsperaMinutos), 1),
                DentroDoTempo = dentroDoTempo,
                ForaDoTempo = foraDoTempo,
                PercentualDentroDoTempo = atendidos.Count == 0
                    ? 0
                    : Math.Round(dentroDoTempo * 100.0 / atendidos.Count, 1),
                PorClassificacao = porClassificacao
            };
        }

        private static double TempoEsperaMinutos(FilaAtendimento fila) =>
            (fila.DataSaida!.Value - fila.DataEntrada).TotalMinutes;

        private static bool ExcedeuTempo(FilaAtendimento fila) =>
            TempoEsperaMinutos(fila) > fila.Triagem.Classificacao.TempoMaximoEsperaMinutos();

        private static DateTime ParaUtc(DateTime data) =>
            data.Kind == DateTimeKind.Utc ? data : DateTime.SpecifyKind(data, DateTimeKind.Utc);
    }
}
