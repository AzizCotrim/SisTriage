using FluentAssertions;
using Moq;
using SisTriage.Application.Services;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;
using SisTriage.UnitTests.Helpers;

namespace SisTriage.UnitTests.Services
{
    public class FilaAtendimentoServiceTests
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IFilaAtendimentoRepository> _filas = new();
        private readonly FilaAtendimentoService _sut;

        public FilaAtendimentoServiceTests()
        {
            _uow.SetupGet(u => u.Filas).Returns(_filas.Object);
            _sut = new FilaAtendimentoService(_uow.Object, MapperFactory.Criar());
        }

        // ---------- ChamarProximo ----------

        [Fact]
        public async Task ChamarProximo_FilaVazia_DeveRetornarNuloSemSalvar()
        {
            _filas.Setup(f => f.ObterFilaOrdenadaPorPrioridadeAsync())
                  .ReturnsAsync(Array.Empty<FilaAtendimento>());

            var resultado = await _sut.ChamarProximoAsync();

            resultado.Should().BeNull();
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task ChamarProximo_SoExisteQuemJaFoiChamado_DeveRetornarNulo()
        {
            var chamado = TestData.Fila(StatusFila.Chamado);
            _filas.Setup(f => f.ObterFilaOrdenadaPorPrioridadeAsync())
                  .ReturnsAsync(new[] { chamado });

            var resultado = await _sut.ChamarProximoAsync();

            resultado.Should().BeNull();
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task ChamarProximo_DeveIgnorarChamadosEPegarOPrimeiroAguardando()
        {
            var jaChamado = TestData.Fila(StatusFila.Chamado);
            var proximo = TestData.Fila(StatusFila.Aguardando, ClassificacaoRisco.Laranja);
            var outro = TestData.Fila(StatusFila.Aguardando, ClassificacaoRisco.Verde);
            // A ordem aqui simula o retorno já ordenado do repositório.
            _filas.Setup(f => f.ObterFilaOrdenadaPorPrioridadeAsync())
                  .ReturnsAsync(new[] { jaChamado, proximo, outro });

            var resultado = await _sut.ChamarProximoAsync();

            resultado.Should().NotBeNull();
            resultado!.Id.Should().Be(proximo.Id);
            resultado.Status.Should().Be(StatusFila.Chamado);
            proximo.Status.Should().Be(StatusFila.Chamado);
            proximo.Triagem.Status.Should().Be(StatusTriagem.EmAtendimento);
            outro.Status.Should().Be(StatusFila.Aguardando);
            jaChamado.Status.Should().Be(StatusFila.Chamado);
            _uow.Verify(u => u.SalvarAsync(), Times.Once);
        }

        // ---------- Finalizar ----------

        [Fact]
        public async Task Finalizar_RegistroInexistente_DeveLancarRecursoNaoEncontrado()
        {
            _filas.Setup(f => f.ObterComTriagemEPacienteAsync(It.IsAny<Guid>()))
                  .ReturnsAsync((FilaAtendimento?)null);

            var act = () => _sut.FinalizarAsync(Guid.NewGuid());

            await act.Should().ThrowAsync<RecursoNaoEncontradoException>();
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Theory]
        [InlineData(StatusFila.Aguardando)]
        [InlineData(StatusFila.Atendido)]
        [InlineData(StatusFila.NaoCompareceu)]
        public async Task Finalizar_SemEstarChamado_DeveLancarOperacaoInvalida(StatusFila statusAtual)
        {
            var fila = TestData.Fila(statusAtual);
            _filas.Setup(f => f.ObterComTriagemEPacienteAsync(fila.Id)).ReturnsAsync(fila);

            var act = () => _sut.FinalizarAsync(fila.Id);

            await act.Should().ThrowAsync<OperacaoInvalidaException>();
            fila.Status.Should().Be(statusAtual);
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task Finalizar_QuemEstaChamado_DeveMarcarComoAtendidoEFinalizarTriagem()
        {
            var fila = TestData.Fila(StatusFila.Chamado);
            fila.Triagem.Status = StatusTriagem.EmAtendimento;
            _filas.Setup(f => f.ObterComTriagemEPacienteAsync(fila.Id)).ReturnsAsync(fila);

            var antes = DateTime.UtcNow;
            var resultado = await _sut.FinalizarAsync(fila.Id);

            resultado.Status.Should().Be(StatusFila.Atendido);
            fila.DataSaida.Should().NotBeNull().And.BeOnOrAfter(antes);
            fila.Triagem.Status.Should().Be(StatusTriagem.Finalizada);
            _uow.Verify(u => u.SalvarAsync(), Times.Once);
        }

        // ---------- Não comparecimento ----------

        [Fact]
        public async Task NaoComparecimento_RegistroInexistente_DeveLancarRecursoNaoEncontrado()
        {
            _filas.Setup(f => f.ObterComTriagemEPacienteAsync(It.IsAny<Guid>()))
                  .ReturnsAsync((FilaAtendimento?)null);

            var act = () => _sut.RegistrarNaoComparecimentoAsync(Guid.NewGuid());

            await act.Should().ThrowAsync<RecursoNaoEncontradoException>();
        }

        [Fact]
        public async Task NaoComparecimento_QuemAindaAguarda_DeveLancarOperacaoInvalida()
        {
            var fila = TestData.Fila(StatusFila.Aguardando);
            _filas.Setup(f => f.ObterComTriagemEPacienteAsync(fila.Id)).ReturnsAsync(fila);

            var act = () => _sut.RegistrarNaoComparecimentoAsync(fila.Id);

            await act.Should().ThrowAsync<OperacaoInvalidaException>();
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task NaoComparecimento_QuemFoiChamado_DeveCancelarTriagem()
        {
            var fila = TestData.Fila(StatusFila.Chamado);
            _filas.Setup(f => f.ObterComTriagemEPacienteAsync(fila.Id)).ReturnsAsync(fila);

            var resultado = await _sut.RegistrarNaoComparecimentoAsync(fila.Id);

            resultado.Status.Should().Be(StatusFila.NaoCompareceu);
            fila.DataSaida.Should().NotBeNull();
            fila.Triagem.Status.Should().Be(StatusTriagem.Cancelada);
            _uow.Verify(u => u.SalvarAsync(), Times.Once);
        }

        // ---------- Mapeamento da resposta ----------

        [Fact]
        public async Task ObterFilaOrdenada_DeveMapearTempoMaximoEExcedido()
        {
            // Laranja tem tempo-alvo de 10 min; 30 min esperando excede.
            var fila = TestData.Fila(StatusFila.Aguardando, ClassificacaoRisco.Laranja,
                entrada: DateTime.UtcNow.AddMinutes(-30));
            _filas.Setup(f => f.ObterFilaOrdenadaPorPrioridadeAsync()).ReturnsAsync(new[] { fila });

            var resultado = (await _sut.ObterFilaOrdenadaAsync()).Single();

            resultado.TempoMaximoEsperaMinutos.Should().Be(10);
            resultado.TempoEsperaMinutos.Should().BeGreaterOrEqualTo(30);
            resultado.TempoExcedido.Should().BeTrue();
            resultado.PacienteNome.Should().Be(fila.Triagem.Paciente.Nome);
        }
    }
}
