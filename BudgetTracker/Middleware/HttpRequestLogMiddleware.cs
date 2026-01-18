using BudgetTracker.Data;
using System.Diagnostics;

namespace BudgetTracker.Middleware;

/// <summary>
/// Logs all of the HTTP request for the server
/// </summary>
/// <param name="dbContext"></param>
public class HttpRequestLogMiddleware(IAppDbContext dbContext) : IMiddleware
{
    private readonly IAppDbContext _dbContext = dbContext;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopWatch = Stopwatch.StartNew();
        await next(context);
        stopWatch.Stop();

        if (!context.Request.Path.StartsWithSegments("/lib"))
        {
            // Build the full request URL
            UriBuilder uri = new()
            {
                Scheme = context.Request.Scheme,
                Host = context.Request.Host.Host,
                Path = context.Request.Path,
                Query = context.Request.QueryString.ToString()
            };

            // Generate the sql entry
            string sql = "INSERT INTO HttpRequestLogs (RequestUrl, RequestMethod, UserAgent, ClientIpAddress, ReferrerUrl, ResponseStatusCode, ResponseTime, DateTimeStamp) " +
                         "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})";

            await _dbContext.ExecuteSqlRawAsync(sql,
                uri.ToString(),
                context.Request.Method,
                context.Request.Headers.UserAgent,
                context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                context.Request.Headers.Referer,
                context.Response.StatusCode,
                stopWatch.Elapsed,
                DateTime.UtcNow
            );
        }
    }
}
