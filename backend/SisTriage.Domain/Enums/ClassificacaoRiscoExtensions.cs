using System;
using System.Collections.Generic;
using System.Text;

namespace SisTriage.Domain.Enums
{
    public static class ClassificacaoRiscoExtensions
    {
        public static int TempoMaximoEsperaMinutos(this ClassificacaoRisco classificacao) => classificacao switch
        {
            ClassificacaoRisco.Vermelho => 0,
            ClassificacaoRisco.Laranja => 10,
            ClassificacaoRisco.Amarelo => 60,
            ClassificacaoRisco.Verde => 120,
            ClassificacaoRisco.Azul => 240,
            _ => throw new ArgumentOutOfRangeException(nameof(classificacao))
        };
    }
}
