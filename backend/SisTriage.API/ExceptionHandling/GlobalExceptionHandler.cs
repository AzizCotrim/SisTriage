using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SisTriage.Domain.Exceptions;

namespace SisTriage.API.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Exceções de domínio são fluxo de negócio esperado, não falha do sistema.
            // Registra apenas o tipo, sem a mensagem, que pode conter dado pessoal sensível (LGPD).
            if (exception is DomainException)
            {
                _logger.LogWarning("Regra de negócio violada: {Excecao} em {Metodo} {Rota}",
                    exception.GetType().Name, httpContext.Request.Method, httpContext.Request.Path);
            }
            else
            {
                _logger.LogError(exception, "Erro não tratado em {Metodo} {Rota}",
                    httpContext.Request.Method, httpContext.Request.Path);
            }

            var (statusCode, title, detail) = exception switch
            {
                RegistroDuplicadoException => (StatusCodes.Status409Conflict, "Registro duplicado", exception.Message),
                CredenciaisInvalidasException => (StatusCodes.Status401Unauthorized, "Credenciais inválidas", exception.Message),
                RecursoNaoEncontradoException => (StatusCodes.Status404NotFound, "Recurso não encontrado", exception.Message),
                OperacaoInvalidaException => (StatusCodes.Status400BadRequest, "Operação inválida", exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "Erro interno no servidor", "Ocorreu um erro inesperado. Tente novamente mais tarde.")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
