namespace SisTriage.Application.DTOs.Indicador
{
    public class IndicadoresRequest
    {
        /// <summary>
        /// Início do período. Quando omitido, considera os últimos 30 dias.
        /// </summary>
        public DateTime? Inicio { get; set; }

        /// <summary>
        /// Fim do período. Quando omitido, considera o momento atual.
        /// </summary>
        public DateTime? Fim { get; set; }
    }
}
