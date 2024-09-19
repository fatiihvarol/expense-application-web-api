using Microsoft.AspNetCore.Http;
using Serilog;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestLoggingMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var response = context.Response;

        // Request gövdesini oku ve logla
        var requestBody = await ReadRequestBodyAsync(request);

        // Response gövdesini oku ve logla
        var originalResponseBody = response.Body;
        using var responseBodyStream = new MemoryStream();
        response.Body = responseBodyStream;

        try
        {
            await _next(context);
        }
        finally
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            LogResponseBody(responseBody);
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBody);
        }
    }

    private async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }

    private void LogResponseBody(string responseBody)
    {
        try
        {
            // JSON formatında olup olmadığını kontrol et ve logla
            var jsonDocument = JsonDocument.Parse(responseBody);
            Log.Information("Response Body: {ResponseBody}", jsonDocument.RootElement.ToString());
        }
        catch (JsonException)
        {
            // JSON formatında değilse, raw string olarak logla
            Log.Information("Response Body: {ResponseBody}", responseBody);
        }
    }
}
