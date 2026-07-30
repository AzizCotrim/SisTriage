using AutoMapper;
using SisTriage.Application.DTOs.FilaAtendimento;
using SisTriage.Domain.Entities;

namespace SisTriage.Application.Mappings
{
    public class FilaAtendimentoProfile : Profile
    {
        public FilaAtendimentoProfile()
        {
            CreateMap<FilaAtendimento, FilaAtendimentoResponse>()
                .ForMember(dest => dest.PacienteNome, opt => opt.MapFrom(src => src.Triagem.Paciente.Nome))
                .ForMember(dest => dest.Classificacao, opt => opt.MapFrom(src => src.Triagem.Classificacao));
        }
    }
}
