using FluentAssertions;
using Moq;
using SisTriage.Application.DTOs.Usuario;
using SisTriage.Application.Security;
using SisTriage.Application.Services;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;
using SisTriage.UnitTests.Helpers;

namespace SisTriage.UnitTests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IUsuarioRepository> _usuarios = new();
        private readonly Mock<IPasswordHasher> _hasher = new();
        private readonly Mock<IJwtTokenGenerator> _jwt = new();
        private readonly Mock<IUsuarioLogado> _logado = new();
        private readonly UsuarioService _sut;

        public UsuarioServiceTests()
        {
            _uow.SetupGet(u => u.Usuarios).Returns(_usuarios.Object);
            _sut = new UsuarioService(_uow.Object, MapperFactory.Criar(), _hasher.Object, _jwt.Object, _logado.Object);
        }

        // ---------- Criar ----------

        [Fact]
        public async Task Criar_EmailJaCadastrado_DeveLancarRegistroDuplicado()
        {
            _usuarios.Setup(r => r.ObterPorEmailAsync("a@a.com")).ReturnsAsync(TestData.NovoUsuario("a@a.com"));

            var act = () => _sut.CriarAsync(new CriarUsuarioRequest { Nome = "A", Email = "a@a.com", Senha = "123456" });

            await act.Should().ThrowAsync<RegistroDuplicadoException>();
            _usuarios.Verify(r => r.Add(It.IsAny<Usuario>()), Times.Never);
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task Criar_DadosValidos_DeveGravarHashDaSenhaEUsuarioAtivo()
        {
            _usuarios.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync((Usuario?)null);
            _hasher.Setup(h => h.HashSenha("senha-secreta")).Returns("HASH");
            Usuario? gravado = null;
            _usuarios.Setup(r => r.Add(It.IsAny<Usuario>())).Callback<Usuario>(u => gravado = u);

            var resposta = await _sut.CriarAsync(new CriarUsuarioRequest
            {
                Nome = "Novo",
                Email = "novo@a.com",
                Senha = "senha-secreta",
                Perfil = Perfil.Medico
            });

            gravado.Should().NotBeNull();
            gravado!.SenhaHash.Should().Be("HASH").And.NotBe("senha-secreta");
            gravado.Ativo.Should().BeTrue();
            gravado.Perfil.Should().Be(Perfil.Medico);
            _uow.Verify(u => u.SalvarAsync(), Times.Once);
            resposta.Email.Should().Be("novo@a.com");
        }

        // ---------- Autenticar ----------

        [Fact]
        public async Task Autenticar_EmailInexistente_DeveLancarCredenciaisInvalidas()
        {
            _usuarios.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync((Usuario?)null);

            var act = () => _sut.AutenticarAsync(new LoginRequest { Email = "x@x.com", Senha = "123456" });

            await act.Should().ThrowAsync<CredenciaisInvalidasException>();
            _jwt.Verify(j => j.GerarToken(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        public async Task Autenticar_SenhaIncorreta_DeveLancarCredenciaisInvalidasComMensagemGenerica()
        {
            var usuario = TestData.NovoUsuario();
            _usuarios.Setup(r => r.ObterPorEmailAsync(usuario.Email)).ReturnsAsync(usuario);
            _hasher.Setup(h => h.VerificarSenha("errada", usuario.SenhaHash)).Returns(false);

            var act = () => _sut.AutenticarAsync(new LoginRequest { Email = usuario.Email, Senha = "errada" });

            // A mensagem não diferencia "e-mail inexistente" de "senha errada" (evita enumeração de contas).
            await act.Should().ThrowAsync<CredenciaisInvalidasException>().WithMessage("E-mail ou senha inválidos.");
            _jwt.Verify(j => j.GerarToken(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        public async Task Autenticar_UsuarioInativo_DeveLancarCredenciaisInvalidas()
        {
            var usuario = TestData.NovoUsuario(ativo: false);
            _usuarios.Setup(r => r.ObterPorEmailAsync(usuario.Email)).ReturnsAsync(usuario);
            _hasher.Setup(h => h.VerificarSenha(It.IsAny<string>(), usuario.SenhaHash)).Returns(true);

            var act = () => _sut.AutenticarAsync(new LoginRequest { Email = usuario.Email, Senha = "123456" });

            await act.Should().ThrowAsync<CredenciaisInvalidasException>().WithMessage("*inativo*");
            _jwt.Verify(j => j.GerarToken(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        public async Task Autenticar_CredenciaisValidas_DeveRetornarToken()
        {
            var usuario = TestData.NovoUsuario();
            _usuarios.Setup(r => r.ObterPorEmailAsync(usuario.Email)).ReturnsAsync(usuario);
            _hasher.Setup(h => h.VerificarSenha("certa", usuario.SenhaHash)).Returns(true);
            _jwt.Setup(j => j.GerarToken(usuario)).Returns("TOKEN-JWT");

            var token = await _sut.AutenticarAsync(new LoginRequest { Email = usuario.Email, Senha = "certa" });

            token.Should().Be("TOKEN-JWT");
        }

        // ---------- Atualizar ----------

        [Fact]
        public async Task Atualizar_UsuarioInexistente_DeveLancarRecursoNaoEncontrado()
        {
            _usuarios.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Usuario?)null);

            var act = () => _sut.AtualizarAsync(Guid.NewGuid(),
                new AtualizarUsuarioRequest { Nome = "X", Perfil = Perfil.Medico, Ativo = true });

            await act.Should().ThrowAsync<RecursoNaoEncontradoException>();
        }

        [Fact]
        public async Task Atualizar_PropriaConta_AlterandoPerfil_DeveSerBloqueado()
        {
            var usuario = TestData.NovoUsuario(perfil: Perfil.Recepcionista);
            _usuarios.Setup(r => r.GetByIdAsync(usuario.Id)).ReturnsAsync(usuario);
            _logado.SetupGet(l => l.Id).Returns(usuario.Id);

            var act = () => _sut.AtualizarAsync(usuario.Id,
                new AtualizarUsuarioRequest { Nome = usuario.Nome, Perfil = Perfil.Medico, Ativo = true });

            await act.Should().ThrowAsync<OperacaoInvalidaException>();
            usuario.Perfil.Should().Be(Perfil.Recepcionista);
            _uow.Verify(u => u.SalvarAsync(), Times.Never);
        }

        [Fact]
        public async Task Atualizar_PropriaConta_DesativandoAcesso_DeveSerBloqueado()
        {
            var usuario = TestData.NovoUsuario();
            _usuarios.Setup(r => r.GetByIdAsync(usuario.Id)).ReturnsAsync(usuario);
            _logado.SetupGet(l => l.Id).Returns(usuario.Id);

            var act = () => _sut.AtualizarAsync(usuario.Id,
                new AtualizarUsuarioRequest { Nome = usuario.Nome, Perfil = usuario.Perfil, Ativo = false });

            await act.Should().ThrowAsync<OperacaoInvalidaException>();
            usuario.Ativo.Should().BeTrue();
        }

        [Fact]
        public async Task Atualizar_PropriaConta_AlterandoSoONome_DevePermitir()
        {
            var usuario = TestData.NovoUsuario();
            _usuarios.Setup(r => r.GetByIdAsync(usuario.Id)).ReturnsAsync(usuario);
            _logado.SetupGet(l => l.Id).Returns(usuario.Id);

            var resposta = await _sut.AtualizarAsync(usuario.Id,
                new AtualizarUsuarioRequest { Nome = "Nome Novo", Perfil = usuario.Perfil, Ativo = usuario.Ativo });

            resposta.Nome.Should().Be("Nome Novo");
            _uow.Verify(u => u.SalvarAsync(), Times.Once);
        }

        [Fact]
        public async Task Atualizar_OutroUsuario_PodeAlterarPerfilEAtivo()
        {
            var alvo = TestData.NovoUsuario(perfil: Perfil.Enfermeiro);
            _usuarios.Setup(r => r.GetByIdAsync(alvo.Id)).ReturnsAsync(alvo);
            _logado.SetupGet(l => l.Id).Returns(Guid.NewGuid()); // quem está logado é outra pessoa

            var resposta = await _sut.AtualizarAsync(alvo.Id,
                new AtualizarUsuarioRequest { Nome = alvo.Nome, Perfil = Perfil.Medico, Ativo = false });

            resposta.Perfil.Should().Be(Perfil.Medico);
            resposta.Ativo.Should().BeFalse();
            _uow.Verify(u => u.SalvarAsync(), Times.Once);
        }
    }
}
