using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Venice.Orders.Domain.Exceptions;

namespace Venice.Orders.Api.Middlewares
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
            _logger.LogError(exception, "Erro capturado pelo middleware: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            switch (exception)
            {
                case ValidationException validationEx:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Erro de Validação";
                    problemDetails.Detail = "Um ou mais erros de validação ocorreram.";
                    problemDetails.Extensions["errors"] = validationEx.Errors
                        .Select(e => new { e.PropertyName, e.ErrorMessage });
                    break;

                case DomainException ex:
                    problemDetails.Status = (int)HttpStatusCode.UnprocessableEntity;
                    problemDetails.Title = "Erro ao processar esta requisição";
                    problemDetails.Detail = ex.Message;
                    break;

                default:
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Title = "Erro de Servidor";
                    problemDetails.Detail = "Ocorreu um erro interno inesperado.";
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}