using FluentAssertions;
using Moq;
using SisTriage.Application.DTOs.Triagem;
using SisTriage.Application.Services;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;
using SisTriage.UnitTests.Helpers;

namespace SisTriage.UnitTests.Services
{
    public class TriagemServiceTests
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IPacienteRepository> _pacientes = new();
        private readonly Mock<IUsuarioRepository> _usuarios = new();
        private readonly Mock<ITriagemRepository> _triagens = new();
        private readonly Mock<IFilaAtendimentoRepository> _filas = new();
        private readonly TriagemService _sut;

        public TriagemServiceTests()
        {
            _uow.SetupGet(u => u.Pacientes).Returns(_pacientes.Object);
            _uow.SetupGet(u => u.Usuarios).Returns(_usuarios.Object);
            _uow.SetupGet(u => u.Triagens).Returns(_triagens.Object);
            _uow.SetupGet(u => u.Filas).Returns(_filas.Object);
            _sut = new TriagemService(_uow.Object, MapperFactory.Criar());
        }

        private static CriarTriagemRequest NovaRequisicao(Guid pacienteId, Guid usuarioId) => new()
        {
            PacienteId = pacienteId,
            UsuarioId = usuarioId,
            Classificacao = ClassificacaoRisco.Laranja,
            QueixaPrincipal = "Dor no peito",
            SinaisVitais = new SinaisVitaisRequest
            {
                PressaoArterialSistolica = 150,
                PressaoArterialDiastolica = 95,
                FrequenciaCardiaca = 110,
                FrequenciaRespiratoria = 22,
                Temperatura = 37.2m,
                SpO2 = 94,
                EscalaDor = 8
            }
        };

        [Fact]
        public async Task Registrar_PacienteInexistente_DeveLancarRecursoNaoEncontradoSemPersistir()
        {
            _pacientes.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Paciente?)null);

            var act = () => _sut.RegistrarAsync(NovaRequisicao(Guid.NewGuid(), Guid.NewGuid()));

            await act.Should().ThrowAsync<RecursoNaoEncontradoException>().WithMessage("*Paciente*");
            _triagens.Verify(r => r.Add(It.IsAny<Triagem>()), Times.Never);
            _filas.Verify(r => r.Add(It.IsAny<FilaAtendimento>()), Times.Never);
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task Registrar_UsuarioInexistente_DeveLancarRecursoNaoEncontradoSemPersistir()
        {
            var paciente = TestData.NovoPaciente();
            _pacientes.Setup(r => r.GetByIdAsync(paciente.Id)).ReturnsAsync(paciente);
            _usuarios.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario?)null);

            var act = () => _sut.RegistrarAsync(NovaRequisicao(paciente.Id, Guid.NewGuid()));

            await act.Should().ThrowAsync<RecursoNaoEncontradoException>().WithMessage("*Usuário*");
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task Registrar_DadosValidos_DeveCriarTriagemEEntradaNaFilaNaMesmaTransacao()
        {
            var paciente = TestData.NovoPaciente();
            var usuario = TestData.NovoUsuario();
            _pacientes.Setup(r => r.GetByIdAsync(paciente.Id)).ReturnsAsync(paciente);
            _usuarios.Setup(r => r.GetByIdAsync(usuario.Id)).ReturnsAsync(usuario);

            Triagem? triagemCriada = null;
            FilaAtendimento? filaCriada = null;
            _triagens.Setup(r => r.Add(It.IsAny<Triagem>())).Callback<Triagem>(t => triagemCriada = t);
            _filas.Setup(r => r.Add(It.IsAny<FilaAtendimento>())).Callback<FilaAtendimento>(f => filaCriada = f);
            _triagens.Setup(r => r.ObterComPacienteEUsuarioAsync(It.IsAny<Guid>()))
                     .ReturnsAsync(() =>
                     {
                         triagemCriada!.Paciente = paciente;
                         triagemCriada.Usuario = usuario;
                         return triagemCriada;
                     });

            var antes = DateTime.UtcNow;
            var resposta = await _sut.RegistrarAsync(NovaRequisicao(paciente.Id, usuario.Id));

            // Triagem nasce aguardando e com a data definida pelo servidor.
            triagemCriada.Should().NotBeNull();
            triagemCriada!.Status.Should().Be(StatusTriagem.Aguardando);
            triagemCriada.DataHora.Should().BeOnOrAfter(antes);
            triagemCriada.Classificacao.Should().Be(ClassificacaoRisco.Laranja);
            triagemCriada.SinaisVitais.SpO2.Should().Be(94);

            // A fila é criada ligada à triagem, aguardando.
            filaCriada.Should().NotBeNull();
            filaCriada!.TriagemId.Should().Be(triagemCriada.Id);
            filaCriada.Status.Should().Be(StatusFila.Aguardando);

            // Atomicidade: uma única chamada de SalvarAsync para as duas escritas.
            _uow.Verify(u => u.SalvarAsync(), Times.Once);

            resposta.PacienteNome.Should().Be(paciente.Nome);
            resposta.UsuarioNome.Should().Be(usuario.Nome);
        }

        [Fact]
        public async Task ObterPorId_Inexistente_DeveRetornarNulo()
        {
            _triagens.Setup(r => r.ObterComPacienteEUsuarioAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((Triagem?)null);

            var resultado = await _sut.ObterPorIdAsync(Guid.NewGuid());

            resultado.Should().BeNull();
        }
    }
}
