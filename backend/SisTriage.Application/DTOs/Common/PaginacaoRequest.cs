using System.ComponentModel.DataAnnotations;

namespace SisTriage.Application.DTOs.Common
{
    public class PaginacaoRequest
    {
        private const int TamanhoMaximo = 100;
        private int _tamanhoPagina = 20;

        [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior ou igual a 1.")]
        public int Pagina { get; set; } = 1;

        /// <summary>
        /// Limitado a 100 para impedir que um cliente peça a base inteira numa requisição.
        /// </summary>
        [Range(1, TamanhoMaximo, ErrorMessage = "O tamanho da página deve estar entre 1 e 100.")]
        public int TamanhoPagina
        {
            get => _tamanhoPagina;
            set => _tamanhoPagina = value > TamanhoMaximo ? TamanhoMaximo : value;
        }
    }
}
