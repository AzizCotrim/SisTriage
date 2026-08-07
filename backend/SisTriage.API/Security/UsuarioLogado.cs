using SisTriage.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SisTriage.API.Security
{
    public class UsuarioLogado : IUsuarioLogado
    {
        private const string UsuarioAnonimo = "sistema";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioLogado(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? Id
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return Guid.TryParse(claim, out var id) ? id : null;
            }
        }

        public string Email
        {
            get
            {
                var usuario = _httpContextAccessor.HttpContext?.User;

                return usuario?.FindFirst(ClaimTypes.Email)?.Value
                    ?? usuario?.FindFirst(JwtRegisteredClaimNames.Email)?.Value
                    ?? UsuarioAnonimo;
            }
        }
    }
}
