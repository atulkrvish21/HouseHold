using System.Net;

namespace HHSurvey.Service
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
      
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
             var traceId = context.TraceIdentifier;
            _logger.LogError(exception,
        "Unhandled exception | TraceId: {TraceId} | Path: {Path}",
        traceId,
        context.Request.Path);


            //More log stuff        

            ExceptionResponse response = exception switch
            {
                ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
                UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
            };
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await LogBadRequestAsync(context, exception);
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }


        private async Task LogBadRequestAsync(HttpContext context, Exception exception)
        {
            var request = context.Request;
            string requestBody = await GetRequestBodyAsync(request);

            // Log the bad request with relevant information.
            _logger.LogWarning(
                "Bad Request (400) detected. Path: {path}, Query: {query}, Body: {body}, Exception: {exceptionMessage}",
                request.Path,
                request.QueryString,
                requestBody,
                exception.Message
            );
        }

        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            // Enable the request body to be buffered so it can be read multiple times.
            request.EnableBuffering();

            // Read the body content.
            var reader = new StreamReader(request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            // Reset the position of the stream so other components can read it.
            request.Body.Position = 0;

            return body;
        }
    }

    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
}
