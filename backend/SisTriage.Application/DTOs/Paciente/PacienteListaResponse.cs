using SisTriage.Domain.Enums;

namespace SisTriage.Application.DTOs.Paciente
{
    /// <summary>
    /// Versão reduzida usada na listagem. O CPF vem mascarado por se tratar de
    /// dado pessoal sensível (LGPD, art. 11) — o valor completo só é exposto na
    /// consulta individual do paciente.
    /// </summary>
    public class PacienteListaResponse
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public Sexo Sexo { get; set; }
    }
}
