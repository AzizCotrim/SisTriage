using AutoMapper;
using SisTriage.Application.DTOs.Common;
using SisTriage.Application.DTOs.Usuario;
using SisTriage.Application.Security;
using SisTriage.Application.Services.Interfaces;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;

namespace SisTriage.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUsuarioLogado _usuarioLogado;

        public UsuarioService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IUsuarioLogado usuarioLogado)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _usuarioLogado = usuarioLogado;
        }

        public async Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request)
        {
            var usuarioExistente = await _unitOfWork.Usuarios.ObterPorEmailAsync(request.Email);
            if (usuarioExistente != null)
                throw new RegistroDuplicadoException("Já existe um usuário cadastrado com esse e-mail.");

            var usuario = _mapper.Map<Usuario>(request);
            usuario.SenhaHash = _passwordHasher.HashSenha(request.Senha);
            usuario.Ativo = true;

            _unitOfWork.Usuarios.Add(usuario);
            await _unitOfWork.SalvarAsync();

            return _mapper.Map<UsuarioResponse>(usuario);
        }

        public async Task<string> AutenticarAsync(LoginRequest request)
        {
            var usuario = await _unitOfWork.Usuarios.ObterPorEmailAsync(request.Email);

            if (usuario == null || !_passwordHasher.VerificarSenha(request.Senha, usuario.SenhaHash))
                throw new CredenciaisInvalidasException("E-mail ou senha inválidos.");

            if (!usuario.Ativo)
                throw new CredenciaisInvalidasException("Usuário inativo.");

            return _jwtTokenGenerator.GerarToken(usuario);
        }

        public async Task<ResultadoPaginado<UsuarioResponse>> ListarAsync(PaginacaoRequest paginacao)
        {
            var (itens, total) = await _unitOfWork.Usuarios.ObterPaginadoAsync(paginacao.Pagina, paginacao.TamanhoPagina);

            return new ResultadoPaginado<UsuarioResponse>
            {
                Itens = _mapper.Map<IEnumerable<UsuarioResponse>>(itens),
                Pagina = paginacao.Pagina,
                TamanhoPagina = paginacao.TamanhoPagina,
                TotalRegistros = total
            };
        }

        public async Task<UsuarioResponse?> ObterPorIdAsync(Guid id)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);

            return usuario == null ? null : _mapper.Map<UsuarioResponse>(usuario);
        }

        public async Task<UsuarioResponse> AtualizarAsync(Guid id, AtualizarUsuarioRequest request)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
            if (usuario == null)
                throw new RecursoNaoEncontradoException("Usuário não encontrado.");

            // Impede que o próprio usuário eleve o seu perfil ou reative o seu acesso.
            // Sem isso, quem pode administrar usuários poderia se autopromover.
            var alterandoProprioAcesso = _usuarioLogado.Id == id
                && (usuario.Perfil != request.Perfil || usuario.Ativo != request.Ativo);

            if (alterandoProprioAcesso)
                throw new OperacaoInvalidaException("Não é possível alterar o próprio perfil ou a própria situação de acesso.");

            usuario.Nome = request.Nome;
            usuario.Perfil = request.Perfil;
            usuario.Ativo = request.Ativo;

            await _unitOfWork.SalvarAsync();

            return _mapper.Map<UsuarioResponse>(usuario);
        }
    }
}