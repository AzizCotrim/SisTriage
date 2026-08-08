namespace SisTriage.Application.DTOs.Common
{
    public class ResultadoPaginado<T>
    {
        public required IEnumerable<T> Itens { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalRegistros { get; set; }

        public int TotalPaginas => TamanhoPagina == 0
            ? 0
            : (int)Math.Ceiling(TotalRegistros / (double)TamanhoPagina);
    }
}
