using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.ValueObjects;

namespace SisTriage.UnitTests.Helpers
{
    /// <summary>
    /// Fábrica de entidades válidas para os testes. Centraliza os campos
    /// obrigatórios (required) para que cada teste só declare o que importa para ele.
    /// </summary>
    public static class TestData
    {
        public static Paciente NovoPaciente(string nome = "Maria da Silva", string cpf = "12345678901") => new()
        {
            Nome = nome,
            CPF = cpf,
            DataNascimento = new DateTime(1990, 5, 10, 0, 0, 0, DateTimeKind.Utc),
            Sexo = default,
            Telefone = "17999999999"
        };

        public static Usuario NovoUsuario(
            string email = "enfermeiro@sistriage.local",
            Perfil perfil = Perfil.Enfermeiro,
            bool ativo = true) => new()
        {
            Nome = "Enfermeiro Teste",
            Email = email,
            SenhaHash = "hash-armazenado",
            Perfil = perfil,
            Ativo = ativo
        };

        public static SinaisVitais NovoSinaisVitais() => new()
        {
            PressaoArterialSistolica = 120,
            PressaoArterialDiastolica = 80,
            FrequenciaCardiaca = 75,
            FrequenciaRespiratoria = 16,
            Temperatura = 36.5m,
            SpO2 = 98,
            EscalaDor = 2
        };

        public static Triagem NovaTriagem(
            ClassificacaoRisco classificacao = ClassificacaoRisco.Amarelo,
            StatusTriagem status = StatusTriagem.Aguardando,
            Paciente? paciente = null)
        {
            paciente ??= NovoPaciente();

            return new Triagem
            {
                PacienteId = paciente.Id,
                Paciente = paciente,
                UsuarioId = Guid.NewGuid(),
                DataHora = DateTime.UtcNow,
                Classificacao = classificacao,
                QueixaPrincipal = "Dor no peito",
                Status = status,
                SinaisVitais = NovoSinaisVitais()
            };
        }

        public static FilaAtendimento Fila(
            StatusFila status = StatusFila.Aguardando,
            ClassificacaoRisco classificacao = ClassificacaoRisco.Amarelo,
            DateTime? entrada = null,
            DateTime? saida = null)
        {
            var triagem = NovaTriagem(classificacao);

            return new FilaAtendimento
            {
                TriagemId = triagem.Id,
                Triagem = triagem,
                DataEntrada = entrada ?? DateTime.UtcNow.AddMinutes(-10),
                DataSaida = saida,
                Status = status
            };
        }
    }
}
