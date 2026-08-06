using AutoMapper;
using SisTriage.Application.DTOs.Paciente;
using SisTriage.Application.Services.Interfaces;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Exceptions;
using SisTriage.Domain.Interfaces;

namespace SisTriage.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PacienteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PacienteResponse> CriarAsync(CriarPacienteRequest request)
        {
            var pacienteExistente = await _unitOfWork.Pacientes.ObterPorCpfAsync(request.CPF);
            if (pacienteExistente != null)
                throw new RegistroDuplicadoException("Já existe um paciente cadastrado com esse CPF.");

            var paciente = _mapper.Map<Paciente>(request);

            _unitOfWork.Pacientes.Add(paciente);
            await _unitOfWork.SalvarAsync();

            return _mapper.Map<PacienteResponse>(paciente);
        }

        public async Task<PacienteResponse?> ObterPorIdAsync(Guid id)
        {
            var paciente = await _unitOfWork.Pacientes.GetByIdAsync(id);

            return paciente == null ? null : _mapper.Map<PacienteResponse>(paciente);
        }

        public async Task<IEnumerable<PacienteResponse>> ListarAsync()
        {
            var pacientes = await _unitOfWork.Pacientes.GetAllAsync();

            return _mapper.Map<IEnumerable<PacienteResponse>>(pacientes);
        }
    }
}