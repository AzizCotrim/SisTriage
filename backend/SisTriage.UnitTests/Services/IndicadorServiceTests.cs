using FluentAssertions;
using Moq;
using SisTriage.Application.DTOs.Indicador;
using SisTriage.Application.Services;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;
using SisTriage.UnitTests.Helpers;

namespace SisTriage.UnitTests.Services
{
    public class IndicadorServiceTests
    {
        private static readonly DateTime Base = new(2026, 10, 1, 12, 0, 0, DateTimeKind.Utc);

        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IFilaAtendimentoRepository> _filas = new();
        private readonly IndicadorService _sut;

        public IndicadorServiceTests()
        {
            _uow.SetupGet(u => u.Filas).Returns(_filas.Object);
            _sut = new IndicadorService(_uow.Object);
        }

        private void RepositorioRetorna(params FilaAtendimento[] registros) =>
            _filas.Setup(f => f.ObterPorPeriodoAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                  .ReturnsAsync(registros);

        private static IndicadoresRequest Periodo() => new()
        {
            Inicio = Base.AddDays(-1),
            Fim = Base.AddDays(1)
        };

        [Fact]
        public async Task Obter_InicioMaiorQueFim_DeveLancarOperacaoInvalidaSemConsultar()
        {
            var request = new IndicadoresRequest { Inicio = Base.AddDays(1), Fim = Base };

            var act = () => _sut.ObterAsync(request);

            await act.Should().ThrowAsync<OperacaoInvalidaException>();
            _filas.Verify(f => f.ObterPorPeriodoAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task Obter_DatasSemKind_DevemSerConvertidasParaUtc()
        {
            RepositorioRetorna();
            var request = new IndicadoresRequest
            {
                Inicio = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Unspecified),
                Fim = new DateTime(2026, 10, 2, 0, 0, 0, DateTimeKind.Unspecified)
            };

            var resposta = await _sut.ObterAsync(request);

            resposta.Inicio.Kind.Should().Be(DateTimeKind.Utc);
            resposta.Fim.Kind.Should().Be(DateTimeKind.Utc);
            _filas.Verify(f => f.ObterPorPeriodoAsync(
                It.Is<DateTime>(d => d.Kind == DateTimeKind.Utc),
                It.Is<DateTime>(d => d.Kind == DateTimeKind.Utc)), Times.Once);
        }

        [Fact]
        public async Task Obter_SemRegistros_DeveRetornarZerosSemDividirPorZero()
        {
            RepositorioRetorna();

            var resposta = await _sut.ObterAsync(Periodo());

            resposta.TotalRegistros.Should().Be(0);
            resposta.Atendidos.Should().Be(0);
            resposta.TempoMedioEsperaMinutos.Should().Be(0);
            resposta.PercentualDentroDoTempo.Should().Be(0);
            resposta.PorClassificacao.Should().BeEmpty();
        }

        [Fact]
        public async Task Obter_ComRegistrosMistos_DeveCalcularIndicadoresGerais()
        {
            RepositorioRetorna(
                // Verde (alvo 120 min): 30 min -> dentro
                TestData.Fila(StatusFila.Atendido, ClassificacaoRisco.Verde, Base, Base.AddMinutes(30)),
                // Verde: 150 min -> fora
                TestData.Fila(StatusFila.Atendido, ClassificacaoRisco.Verde, Base, Base.AddMinutes(150)),
                // Vermelho (alvo 0 min): 5 min -> fora
                TestData.Fila(StatusFila.Atendido, ClassificacaoRisco.Vermelho, Base, Base.AddMinutes(5)),
                // Não atendidos / ainda na fila não entram nas médias
                TestData.Fila(StatusFila.NaoCompareceu, ClassificacaoRisco.Amarelo, Base, Base.AddMinutes(90)),
                TestData.Fila(StatusFila.Aguardando, ClassificacaoRisco.Azul, Base),
                TestData.Fila(StatusFila.Chamado, ClassificacaoRisco.Laranja, Base));

            var resposta = await _sut.ObterAsync(Periodo());

            resposta.TotalRegistros.Should().Be(6);
            resposta.Atendidos.Should().Be(3);
            resposta.NaoComparecimentos.Should().Be(1);
            resposta.AindaNaFila.Should().Be(2);
            resposta.DentroDoTempo.Should().Be(1);
            resposta.ForaDoTempo.Should().Be(2);
            resposta.PercentualDentroDoTempo.Should().Be(33.3);
            resposta.TempoMedioEsperaMinutos.Should().Be(61.7); // (30 + 150 + 5) / 3
        }

        [Fact]
        public async Task Obter_DeveAgruparPorClassificacaoNaOrdemDeUrgencia()
        {
            RepositorioRetorna(
                TestData.Fila(StatusFila.Atendido, ClassificacaoRisco.Verde, Base, Base.AddMinutes(30)),
                TestData.Fila(StatusFila.Atendido, ClassificacaoRisco.Verde, Base, Base.AddMinutes(150)),
                TestData.Fila(StatusFila.Atendido, ClassificacaoRisco.Vermelho, Base, Base.AddMinutes(5)));

            var resposta = await _sut.ObterAsync(Periodo());
            var grupos = resposta.PorClassificacao.ToList();

            grupos.Select(g => g.Classificacao)
                  .Should().ContainInOrder(ClassificacaoRisco.Vermelho, ClassificacaoRisco.Verde);

            var verde = grupos.Single(g => g.Classificacao == ClassificacaoRisco.Verde);
            verde.Quantidade.Should().Be(2);
            verde.TempoMaximoEsperaMinutos.Should().Be(120);
            verde.TempoMedioEsperaMinutos.Should().Be(90);
            verde.DentroDoTempo.Should().Be(1);
            verde.ForaDoTempo.Should().Be(1);
        }

        [Fact]
        public async Task Obter_EsperaExatamenteIgualAoTempoAlvo_DeveContarComoDentroDoTempo()
        {
            // Regra de borda: só excede quando é ESTRITAMENTE maior que o tempo-alvo.
            RepositorioRetorna(
                TestData.Fila(StatusFila.Atendido, ClassificacaoRisco.Amarelo, Base, Base.AddMinutes(60)));

            var resposta = await _sut.ObterAsync(Periodo());

            resposta.DentroDoTempo.Should().Be(1);
            resposta.ForaDoTempo.Should().Be(0);
        }
    }
}
