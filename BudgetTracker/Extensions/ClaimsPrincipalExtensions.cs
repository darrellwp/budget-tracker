using BudgetTracker.Models.Enumerations;
using System.Security.Claims;
using BudgetTracker.Data.Entities;

namespace BudgetTracker.Extensions;

/// <summary>
/// Extends the <see cref="ClaimsPrincipal"/> to make it easier to pull data from the identity
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the <see cref="ApplicationUser.UserId"/>
    /// </summary>
    /// <param name="user"></param>
    /// <returns><see cref="GUID"/> of the <see cref="ApplicationUser"/></returns>
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}
