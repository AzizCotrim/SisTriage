namespace SisTriage.Domain.Interfaces
{
    /// <summary>
    /// Abstrai quem está executando a operação atual, para que as camadas de
    /// domínio e infraestrutura possam registrar auditoria sem depender do ASP.NET Core.
    /// </summary>
    public interface IUsuarioLogado
    {
        Guid? Id { get; }
        string Email { get; }
    }
}
