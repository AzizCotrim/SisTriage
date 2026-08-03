using AutoMapper;
using SisTriage.Application.DTOs.Usuario;
using SisTriage.Domain.Entities;

namespace SisTriage.Application.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioResponse>();
            CreateMap<CriarUsuarioRequest, Usuario>()
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore());
        }
    }
}
