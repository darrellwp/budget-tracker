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


internal class CreateModelTests
{
    private readonly Mock<IUserService> _userService;
    private readonly CreateModel _page;

    public CreateModelTests()
    {
        _userService = new Mock<IUserService>();
        _userService.Setup(x => x.AddCategoryAsync(It.IsAny<CategoryModifyDto>(), It.IsAny<Guid>())).Verifiable();

        _page = new(_userService.Object);
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
        _userService.Verify(x => x.AddCategoryAsync(It.IsAny<CategoryModifyDto>(), It.IsAny<Guid>()), Times.Once);
    }
}
