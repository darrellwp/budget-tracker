using BudgetTracker.Middleware;

namespace BudgetTracker.Extensions;

/// <summary>
/// Middle ware extension methods for <see cref="IApplicationBuilder"/>
/// </summary>
public static class IApplicationBuilderExtensions
{
    /// <summary>
    /// Registers the HTTP request logging middleware
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseHttpRequestLogging(this IApplicationBuilder app) => app.UseMiddleware<HttpRequestLogMiddleware>();
}
