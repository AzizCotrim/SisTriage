namespace SisTriage.Domain.Exceptions
{
    public class CredenciaisInvalidasException : DomainException
    {
        public CredenciaisInvalidasException(string message) : base(message)
        {
        }
    }
}
