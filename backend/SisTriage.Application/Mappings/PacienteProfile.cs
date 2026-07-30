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
        }
    }
}
