namespace SisTriage.Domain.Exceptions
{
    public class OperacaoInvalidaException : DomainException
    {
        public OperacaoInvalidaException(string message) : base(message)
        {
        }
    }
}
