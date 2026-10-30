using FluentAssertions;
using Moq;
using SisTriage.Application.DTOs.Common;
using SisTriage.Application.DTOs.Paciente;
using SisTriage.Application.Services;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;
using SisTriage.UnitTests.Helpers;

namespace SisTriage.UnitTests.Services
{
    public class PacienteServiceTests
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IPacienteRepository> _pacientes = new();
        private readonly Mock<ILogAuditoriaRepository> _logs = new();
        private readonly Mock<IUsuarioLogado> _logado = new();
        private readonly PacienteService _sut;

        public PacienteServiceTests()
        {
            _uow.SetupGet(u => u.Pacientes).Returns(_pacientes.Object);
            _uow.SetupGet(u => u.LogsAuditoria).Returns(_logs.Object);
            _logado.SetupGet(l => l.Id).Returns(Guid.NewGuid());
            _logado.SetupGet(l => l.Email).Returns("recepcao@sistriage.local");
            _sut = new PacienteService(_uow.Object, MapperFactory.Criar(), _logado.Object);
        }

        private static CriarPacienteRequest NovaRequisicao(string cpf = "12345678901") => new()
        {
            Nome = "Maria da Silva",
            CPF = cpf,
            DataNascimento = new DateTime(1990, 5, 10, 0, 0, 0, DateTimeKind.Utc),
            Sexo = default,
            Telefone = "17999999999"
        };

        [Fact]
        public async Task Criar_CpfJaCadastrado_DeveLancarRegistroDuplicado()
        {
            _pacientes.Setup(r => r.ObterPorCpfAsync("12345678901")).ReturnsAsync(TestData.NovoPaciente());

            var act = () => _sut.CriarAsync(NovaRequisicao());

            await act.Should().ThrowAsync<RegistroDuplicadoException>();
            _pacientes.Verify(r => r.Add(It.IsAny<Paciente>()), Times.Never);
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task Criar_CpfNovo_DeveAdicionarESalvarUmaVez()
        {
            _pacientes.Setup(r => r.ObterPorCpfAsync(It.IsAny<string>())).ReturnsAsync((Paciente?)null);

            var resposta = await _sut.CriarAsync(NovaRequisicao());

            _pacientes.Verify(r => r.Add(It.Is<Paciente>(p => p.CPF == "12345678901")), Times.Once);
            _uow.Verify(u => u.SalvarAsync(), Times.Once);
            resposta.Nome.Should().Be("Maria da Silva");
            resposta.CPF.Should().Be("12345678901"); // consulta individual devolve o CPF completo
        }

        [Fact]
        public async Task ObterPorId_Inexistente_DeveRetornarNuloSemRegistrarAuditoria()
        {
            _pacientes.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Paciente?)null);

            var resultado = await _sut.ObterPorIdAsync(Guid.NewGuid());

            resultado.Should().BeNull();
            _logs.Verify(l => l.Add(It.IsAny<LogAuditoria>()), Times.Never);
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task ObterPorId_Existente_DeveRegistrarAuditoriaDeConsulta()
        {
            var paciente = TestData.NovoPaciente();
            _pacientes.Setup(r => r.GetByIdAsync(paciente.Id)).ReturnsAsync(paciente);
            LogAuditoria? log = null;
            _logs.Setup(l => l.Add(It.IsAny<LogAuditoria>())).Callback<LogAuditoria>(l => log = l);

            var resultado = await _sut.ObterPorIdAsync(paciente.Id);

            resultado.Should().NotBeNull();
            log.Should().NotBeNull();
            log!.Acao.Should().Be(AcaoAuditoria.Consulta);
            log.Entidade.Should().Be(nameof(Paciente));
            log.RegistroId.Should().Be(paciente.Id.ToString());
            log.UsuarioEmail.Should().Be("recepcao@sistriage.local");
            _uow.Verify(u => u.SalvarAsync(), Times.Once);
        }

        [Fact]
        public async Task Atualizar_Inexistente_DeveLancarRecursoNaoEncontrado()
        {
            _pacientes.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Paciente?)null);

            var act = () => _sut.AtualizarAsync(Guid.NewGuid(),
                new AtualizarPacienteRequest { Nome = "X", Sexo = default, Telefone = "1" });

            await act.Should().ThrowAsync<RecursoNaoEncontradoException>();
        }

        [Fact]
        public async Task Atualizar_Existente_NaoDeveAlterarOCpf()
        {
            var paciente = TestData.NovoPaciente(cpf: "11122233344");
            _pacientes.Setup(r => r.GetByIdAsync(paciente.Id)).ReturnsAsync(paciente);

            var resposta = await _sut.AtualizarAsync(paciente.Id, new AtualizarPacienteRequest
            {
                Nome = "Nome Corrigido",
                Sexo = default,
                Telefone = "17888888888",
                Alergias = "Dipirona"
            });

            resposta.Nome.Should().Be("Nome Corrigido");
            resposta.Alergias.Should().Be("Dipirona");
            paciente.CPF.Should().Be("11122233344");
            _uow.Verify(u => u.SalvarAsync(), Times.Once);
        }

        // ---------- Listagem (LGPD: CPF mascarado) ----------

        [Fact]
        public async Task Listar_DeveMascararCpfEPreservarPaginacao()
        {
            var paciente = TestData.NovoPaciente(cpf: "12345678901");
            _pacientes.Setup(r => r.ObterPaginadoAsync(2, 10))
                      .ReturnsAsync((new[] { paciente } as IEnumerable<Paciente>, 11));

            var resultado = await _sut.ListarAsync(new PaginacaoRequest { Pagina = 2, TamanhoPagina = 10 });

            resultado.Itens.Single().CPF.Should().Be("***.456.789-**");
            resultado.Pagina.Should().Be(2);
            resultado.TotalRegistros.Should().Be(11);
            resultado.TotalPaginas.Should().Be(2);
        }

        [Fact]
        public async Task Listar_CpfComTamanhoInvalido_DeveMascararTotalmente()
        {
            var paciente = TestData.NovoPaciente(cpf: "123");
            _pacientes.Setup(r => r.ObterPaginadoAsync(1, 20))
                      .ReturnsAsync((new[] { paciente } as IEnumerable<Paciente>, 1));

            var resultado = await _sut.ListarAsync(new PaginacaoRequest());

            resultado.Itens.Single().CPF.Should().Be("***.***.***-**");
        }
    }
}
