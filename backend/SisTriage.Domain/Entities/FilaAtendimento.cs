using SisTriage.Domain.Enums;

namespace SisTriage.Domain.Entities
{
    public class FilaAtendimento
    {
        public Guid Id { get; private set; }
        public required Guid TriagemId { get; set; }
        public required DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public StatusFila Status { get; set; }

        public FilaAtendimento()
        {
            Id = Guid.NewGuid();
        }
    }
}
