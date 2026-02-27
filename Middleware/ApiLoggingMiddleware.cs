using HHSurvey.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class ApiLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ApiLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ApplicationDbContext db)
    {
        var request = context.Request;

        request.EnableBuffering();

        string requestBody = "";
        if (request.ContentLength > 0)
        {
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        // Swap response stream to capture output
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        catch
        {
            // let the exception propagate
            throw;
        }
        finally
        {
            // Read response body
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();

            // Rewind stream before sending to client
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await context.Response.Body.CopyToAsync(originalBodyStream);

            string userId = context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "";
            string ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            string device = context.Request.Headers["User-Agent"].ToString();

            var log = new ApiLog
            {
                UserId = userId,
                Path = request.Path,
                Method = request.Method,
                QueryString = request.QueryString.ToString(),
                Body = requestBody,
                ResponseBody = responseText,
                ResponseStatus = context.Response.StatusCode,
                IpAddress = ip,
                DeviceInfo = device
            };

            db.ApiLog.Add(log);
            await db.SaveChangesAsync();
        }
    }

}
