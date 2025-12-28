using BudgetTracker.Data;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Areas.User.Pages;

[Authorize]
public class IndexModel() : PageModel
{

}
