using SisTriage.Domain.Entities;

namespace SisTriage.Application.Security
{
    public interface IJwtTokenGenerator
    {
        string GerarToken(Usuario usuario);
    }
}
