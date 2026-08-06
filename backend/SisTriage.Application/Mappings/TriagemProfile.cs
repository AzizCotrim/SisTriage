using AutoMapper;
using SisTriage.Application.DTOs.Triagem;
using SisTriage.Domain.Entities;
using SisTriage.Domain.ValueObjects;

namespace SisTriage.Application.Mappings
{
    public class TriagemProfile : Profile
    {
        public TriagemProfile()
        {
            CreateMap<Triagem, TriagemResponse>();
            CreateMap<CriarTriagemRequest, Triagem>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.DataHora, opt => opt.Ignore());

            CreateMap<SinaisVitais, SinaisVitaisResponse>();
            CreateMap<SinaisVitaisRequest, SinaisVitais>();
        }
    }
}
