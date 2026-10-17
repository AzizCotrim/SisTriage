using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SisTriage.Application.Mappings;

namespace SisTriage.UnitTests.Helpers
{
    /// <summary>
    /// Cria o IMapper real da aplicação, registrado do mesmo jeito que no Program.cs.
    /// Usar o mapper verdadeiro (e não um mock) faz os testes de serviço
    /// validarem também os Profiles.
    /// </summary>
    public static class MapperFactory
    {
        public static IMapper Criar()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(cfg => { }, typeof(PacienteProfile).Assembly);

            return services.BuildServiceProvider().GetRequiredService<IMapper>();
        }
    }
}
