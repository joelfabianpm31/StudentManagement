using StudentManagement.Domain.Exceptions;

namespace StudentManagement.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception,
                "Error no controlado en {Path}: {Message}",
                context.Request.Path, exception.Message);

            // Mapea el tipo de excepcion al codigo HTTP correspondiente
            var (statusCode, message) = exception switch
            {
                NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
                BusinessRuleException ex => (StatusCodes.Status400BadRequest, ex.Message),
                _ => (StatusCodes.Status500InternalServerError,
                    "Ocurrio un error inesperado. Por favor intenta de nuevo.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message,
                statusCode
            });
        }
    }

}

