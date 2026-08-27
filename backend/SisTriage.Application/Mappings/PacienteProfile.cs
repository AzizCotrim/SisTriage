using AutoMapper;
using SisTriage.Application.DTOs.Paciente;
using SisTriage.Domain.Entities;

namespace SisTriage.Application.Mappings
{
    public class PacienteProfile : Profile
    {
        public PacienteProfile()
        {
            CreateMap<Paciente, PacienteResponse>();
            CreateMap<CriarPacienteRequest, Paciente>();

            CreateMap<Paciente, PacienteListaResponse>()
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src =>
                    src.CPF.Length == 11
                        ? "***." + src.CPF.Substring(3, 3) + "." + src.CPF.Substring(6, 3) + "-**"
                        : "***.***.***-**"));
        }
    }
}
