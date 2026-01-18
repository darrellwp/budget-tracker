using BudgetTracker.Areas.User.Pages.Categories;
using BudgetTracker.Models.Constants;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Services;
using BudgetTracker.Tests.TestingTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace BudgetTracker.Tests.Areas.User.Categories;

internal class EditModelTests
{
    private readonly Mock<IUserService> _userService;
    private readonly EditModel _page;

    public EditModelTests()
    {
        _userService = new Mock<IUserService>();
        _userService.Setup(x => x.UpdateCategoryAsync(It.IsAny<CategoryModifyDto>(), It.IsAny<Guid>())).Verifiable();

        _page = new(_userService.Object);
    }

    [Test]
    public async Task OnGetAsync_CategoryNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        _userService.Setup(x => x.GetUserCategoryAsync(categoryId, PageModelExtensions.DefaultGuid)).ReturnsAsync((CategoryModifyDto?)null);

        _page.SetLoggedInUser();

        // Act
        var result = await _page.OnGetAsync(categoryId);

        // Assert
        await Assert.That(result).IsTypeOf<NotFoundResult>();
        _userService.Verify(x => x.GetUserCategoryAsync(categoryId, PageModelExtensions.DefaultGuid), Times.Once);
    }

    [Test]
    public async Task OnGetAsync_CategoryFound_ReturnsPageResult()
    {
        // Arrange
        Guid categoryId = Guid.NewGuid();

        CategoryModifyDto dto = new()
        {
            Name = "Test Category",
            Description = "Test Description",
            Icon = "Test Icon",
            MonthlyLimit = 1
        };

        _userService.Setup(x => x.GetUserCategoryAsync(categoryId, PageModelExtensions.DefaultGuid)).ReturnsAsync(dto);

        _page.SetLoggedInUser();

        // Act
        var result = await _page.OnGetAsync(categoryId);

        // Assert
        await Assert.That(result).IsTypeOf<PageResult>();
        await Assert.That(_page.Category.Name).EqualTo(dto.Name);
        await Assert.That(_page.Category.Description).EqualTo(dto.Description);
        await Assert.That(_page.Category.MonthlyLimit).EqualTo(dto.MonthlyLimit);
        await Assert.That(_page.Category.Icon).EqualTo(dto.Icon);
        _userService.Verify(x => x.GetUserCategoryAsync(categoryId, PageModelExtensions.DefaultGuid), Times.Once);
    }

    [Test]
    public async Task OnPostAsync_NullModel_ReturnsBadRequestResult()
    {
        // Arrange

        // Act
        var result = await _page.OnPostAsync();

        // Assert
        await Assert.That(result).IsTypeOf<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_InvalidModel_ReturnsBadRequestResult()
    {

        // Arrange
        _page.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext()
        };

        _page.ModelState.AddModelError("Name", "Invalid Model!");

        // Act
        var result = await _page.OnPostAsync();

        // Assert
        await Assert.That(result).IsTypeOf<BadRequestResult>();
    }

    [Test]
    public async Task OnPostAsync_ValidModel_ReturnsRedirectToPageResult()
    {
        // Arrange
        _page.SetLoggedInUser();
        _page.SetTempData();

        _page.Category = new()
        {
            Name = "Test Category"
        };

        // Act
        var result = await _page.OnPostAsync();

        // Assert
        RedirectToPageResult? redirect = await Assert.That(result).IsTypeOf<RedirectToPageResult>();
        await Assert.That(redirect).IsNotNull();
        await Assert.That(redirect.PageName).IsEqualTo("/Categories/Index");
        await Assert.That(_page.TempData[TempDataKeys.ToastNotification]).IsNotNull();
        _userService.Verify(x => x.UpdateCategoryAsync(It.IsAny<CategoryModifyDto>(), It.IsAny<Guid>()), Times.Once);
    }
}