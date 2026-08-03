namespace SisTriage.Application.Security
{
    public interface IPasswordHasher
    {
        string HashSenha(string senha);
        bool VerificarSenha(string senha, string hash);
    }
}
