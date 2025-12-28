using BudgetTracker.Middleware;

namespace BudgetTracker.Extensions;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseHttpRequestLogging(this IApplicationBuilder app) => app.UseMiddleware<HttpRequestLogMiddleware>();
}
