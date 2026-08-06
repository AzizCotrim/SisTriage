namespace SisTriage.Domain.Exceptions
{
    public class RegistroDuplicadoException : DomainException
    {
        public RegistroDuplicadoException(string message) : base(message)
        {
        }
    }
}
