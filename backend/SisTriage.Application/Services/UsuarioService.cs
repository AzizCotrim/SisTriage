using AutoMapper;
using SisTriage.Application.DTOs.Usuario;
using SisTriage.Application.Security;
using SisTriage.Application.Services.Interfaces;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Interfaces;

namespace SisTriage.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request)
        {
            var usuarioExistente = await _unitOfWork.Usuarios.ObterPorEmailAsync(request.Email);
            if (usuarioExistente != null)
                throw new InvalidOperationException("Já existe um usuário cadastrado com esse e-mail.");

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
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

            return _jwtTokenGenerator.GerarToken(usuario);
        }
    }
}