using FluentAssertions;
using SisTriage.Application.DTOs.Common;

namespace SisTriage.UnitTests.Application
{
    public class PaginacaoTests
    {
        [Fact]
        public void PaginacaoRequest_Padrao_DeveComecarNaPaginaUmCom20Itens()
        {
            var request = new PaginacaoRequest();

            request.Pagina.Should().Be(1);
            request.TamanhoPagina.Should().Be(20);
        }

        [Theory]
        [InlineData(50, 50)]
        [InlineData(100, 100)]
        [InlineData(101, 100)]
        [InlineData(5000, 100)]
        public void PaginacaoRequest_TamanhoPagina_DeveSerLimitadoA100(int informado, int esperado)
        {
            var request = new PaginacaoRequest { TamanhoPagina = informado };

            request.TamanhoPagina.Should().Be(esperado);
        }

        [Theory]
        [InlineData(0, 10, 0)]
        [InlineData(10, 10, 1)]
        [InlineData(11, 10, 2)]
        [InlineData(25, 10, 3)]
        public void ResultadoPaginado_TotalPaginas_DeveArredondarParaCima(int total, int tamanho, int esperado)
        {
            var resultado = new ResultadoPaginado<int>
            {
                Itens = Array.Empty<int>(),
                TotalRegistros = total,
                TamanhoPagina = tamanho
            };

            resultado.TotalPaginas.Should().Be(esperado);
        }

        [Fact]
        public void ResultadoPaginado_TamanhoPaginaZero_NaoDeveDividirPorZero()
        {
            var resultado = new ResultadoPaginado<int>
            {
                Itens = Array.Empty<int>(),
                TotalRegistros = 10,
                TamanhoPagina = 0
            };

            resultado.TotalPaginas.Should().Be(0);
        }
    }
}
