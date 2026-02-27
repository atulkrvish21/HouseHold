using log4net;
using System.Text;

namespace HHSurvey.Service
{
    public class PostRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILog log = LogManager.GetLogger(typeof(PostRequestLoggingMiddleware));

        public PostRequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var logMessage = $"POST Request to {context.Request.Path}\nBody: {body}";
                log.Info(logMessage);
            }

            await _next(context);
        }
    }

}
