using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Security.Claims;

namespace BudgetTracker.Tests.TestingTools;

internal static class PageModelExtensions
{
    public static Guid DefaultGuid = Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA");

    public static void SetTempData(this PageModel pageModel)
    {
        pageModel.TempData = new TempDataDictionary(pageModel.HttpContext, Mock.Of<ITempDataProvider>());
    }

    public static void SetLoggedInUser(this PageModel pageModel)
    { 
        pageModel.PageContext = new()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        [
                            new Claim(ClaimTypes.NameIdentifier, DefaultGuid.ToString())
                        ],
                        authenticationType: "Basic"
                    )
                )
            }
        };
    }

    public static void SetPageContext(this PageModel pageModel)
    {
        pageModel.PageContext = new()
        {
            HttpContext = new DefaultHttpContext()
        };
    }
}
