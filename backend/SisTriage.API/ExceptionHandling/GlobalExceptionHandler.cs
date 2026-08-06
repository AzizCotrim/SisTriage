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
            _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);

            var (statusCode, title, detail) = exception switch
            {
                RegistroDuplicadoException => (StatusCodes.Status409Conflict, "Registro duplicado", exception.Message),
                CredenciaisInvalidasException => (StatusCodes.Status401Unauthorized, "Credenciais inválidas", exception.Message),
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
