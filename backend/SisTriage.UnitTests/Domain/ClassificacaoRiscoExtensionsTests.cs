using FluentAssertions;
using SisTriage.Domain.Enums;

namespace SisTriage.UnitTests.Domain
{
    public class ClassificacaoRiscoExtensionsTests
    {
        [Theory]
        [InlineData(ClassificacaoRisco.Vermelho, 0)]
        [InlineData(ClassificacaoRisco.Laranja, 10)]
        [InlineData(ClassificacaoRisco.Amarelo, 60)]
        [InlineData(ClassificacaoRisco.Verde, 120)]
        [InlineData(ClassificacaoRisco.Azul, 240)]
        public void TempoMaximoEsperaMinutos_DeveSeguirProtocoloDeManchester(
            ClassificacaoRisco classificacao, int esperado)
        {
            classificacao.TempoMaximoEsperaMinutos().Should().Be(esperado);
        }

        [Fact]
        public void TempoMaximoEsperaMinutos_ValorForaDoEnum_DeveLancarExcecao()
        {
            var invalida = (ClassificacaoRisco)999;

            var act = () => invalida.TempoMaximoEsperaMinutos();

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TempoMaximoEsperaMinutos_DeveCrescerConformeMenorUrgencia()
        {
            // Garante a propriedade que a fila depende: quanto menos urgente, maior o tempo-alvo.
            var tempos = new[]
            {
                ClassificacaoRisco.Vermelho, ClassificacaoRisco.Laranja, ClassificacaoRisco.Amarelo,
                ClassificacaoRisco.Verde, ClassificacaoRisco.Azul
            }.Select(c => c.TempoMaximoEsperaMinutos());

            tempos.Should().BeInAscendingOrder();
        }
    }
}
