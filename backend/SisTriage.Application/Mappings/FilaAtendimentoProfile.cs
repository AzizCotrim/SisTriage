using AutoMapper;
using SisTriage.Application.DTOs.FilaAtendimento;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;

namespace SisTriage.Application.Mappings
{
    public class FilaAtendimentoProfile : Profile
    {
        public FilaAtendimentoProfile()
        {
            CreateMap<FilaAtendimento, FilaAtendimentoResponse>()
                .ForMember(dest => dest.PacienteNome, opt => opt.MapFrom(src => src.Triagem.Paciente.Nome))
                .ForMember(dest => dest.Classificacao, opt => opt.MapFrom(src => src.Triagem.Classificacao))
                .ForMember(dest => dest.TempoMaximoEsperaMinutos,
                    opt => opt.MapFrom(src => src.Triagem.Classificacao.TempoMaximoEsperaMinutos()))
                .ForMember(dest => dest.TempoEsperaMinutos,
                    opt => opt.MapFrom(src => (int)((src.DataSaida ?? DateTime.UtcNow) - src.DataEntrada).TotalMinutes))
                .ForMember(dest => dest.TempoExcedido, opt => opt.Ignore());
        }
    }
}
