using Microsoft.EntityFrameworkCore;
using SisTriage.Application.Security;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Infrastructure.Persistence;

namespace SisTriage.API.Seed
{
    public static class DbSeeder
    {
        /// <summary>
        /// Cria o primeiro usuário do sistema caso não exista nenhum.
        /// Necessário porque o cadastro de usuários exige um Recepcionista autenticado.
        /// </summary>
        public static async Task SeedUsuarioInicialAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            if (await context.Usuarios.AnyAsync())
                return;

            var recepcionista = new Usuario
            {
                Nome = "Recepcionista Inicial",
                Email = "recepcao@sistriage.local",
                SenhaHash = passwordHasher.HashSenha("Sistriage@123"),
                Perfil = Perfil.Recepcionista,
                Ativo = true
            };

            context.Usuarios.Add(recepcionista);
            await context.SaveChangesAsync();
        }
    }
}
