using BudgetTracker.Areas.User.Pages.Categories;
using BudgetTracker.Models.Constants;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Services;
using BudgetTracker.Tests.TestingTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BudgetTracker.Tests.Areas.User.Categories;

internal class IndexModelTests
{
    private readonly Mock<IUserService> _userService;
    private readonly IndexModel _page;

    public IndexModelTests()
    {
        _userService = new();

        _page = new(_userService.Object);
    }

    [Test]
    public async Task OnGetAsync_LoadCategories_Void()
    {
        // Arrange
        IEnumerable<CategoryUserListDto> serviceResults = [
            new(){
                Name = "Test"
            },
            new(){
                Name = "Test2"
            }
        ];

        _userService.Setup(x => x.GetUserCategoryListAsync(It.IsAny<Guid>())).ReturnsAsync(serviceResults);

        _page.SetLoggedInUser();

        // Act
        await _page.OnGetAsync();

        // Assert
        await Assert.That(_page.Categories).EqualTo(serviceResults);
    }

    [Test]
    public async Task OnPostRemoveCategoryAsync_CategoryRemoveFailed_ReturnsRedirecToPageResult()
    {
        // Arrange
        _userService.Setup(x => x.RemoveCategoryAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(false);

        _page.SetLoggedInUser();
        _page.SetTempData();

        // Act
        var result = await _page.OnPostRemoveCategoryAsync(Guid.NewGuid());

        // Assert
        RedirectToPageResult? redirectResult = await Assert.That(result).IsTypeOf<RedirectToPageResult>();
        await Assert.That(redirectResult).IsNotNull();
        await Assert.That(redirectResult.PageName).EqualTo(null);
        await Assert.That(_page.TempData[TempDataKeys.ToastNotification]).IsNotNull();
    }

    [Test]
    public async Task OnPostRemoveCategoryAsync_CategoryRemoveSuccess_ReturnsRedirecToPageResult()
    {
        // Arrange
        _userService.Setup(x => x.RemoveCategoryAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

        _page.SetLoggedInUser();
        _page.SetTempData();

        // Act
        var result = await _page.OnPostRemoveCategoryAsync(Guid.NewGuid());

        // Assert
        RedirectToPageResult? redirectResult = await Assert.That(result).IsTypeOf<RedirectToPageResult>();
        await Assert.That(redirectResult).IsNotNull();
        await Assert.That(redirectResult.PageName).EqualTo(null);
        await Assert.That(_page.TempData[TempDataKeys.ToastNotification]).IsNull();
    }
}