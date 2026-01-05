using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Helpers.Web.Middlewares.Diagnostic;

public class DiagnosticMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DiagnosticMiddleware> _logger;

    public DiagnosticMiddleware(RequestDelegate next, ILogger<DiagnosticMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (!_logger.IsEnabled(LogLevel.Debug))
            await _next(httpContext);

        var sw = Stopwatch.StartNew();
        await _next(httpContext);
        sw.Stop();
        _logger.LogDebug("Path: {path} - Elapsed {elapsed}ms", httpContext.Request.Path, sw.ElapsedMilliseconds);
    }
}
