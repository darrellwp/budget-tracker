using BudgetTracker.Models.Enumerations;
using System.Security.Claims;

namespace BudgetTracker.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }

    public static int GetSubscriptionId(this ClaimsPrincipal user)
    {
        var subscriptionIdClaim = user.Claims.FirstOrDefault(c => c.Type == "SubscriptionId");
        return subscriptionIdClaim != null ? int.Parse(subscriptionIdClaim.Value) : (int)SubscriptionTiers.Free;
    }
}
