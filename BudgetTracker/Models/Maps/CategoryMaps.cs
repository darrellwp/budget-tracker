using BudgetTracker.Data.Entities;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.ViewModels;
using System.Globalization;

namespace BudgetTracker.Models.Maps;

/// <summary>
/// Maps for Category models
/// </summary>
public static class CategoryMaps
{
    /// <summary>
    /// Converts the modify DTO to the base entity for adding and updating
    /// </summary>
    /// <param name="dto">Modify DTO</param>
    /// <returns>Entity</returns>
    public static Category ToEntity(this CategoryModifyDto dto)
    {
        return new Category
        {
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Description = dto.Description,
            Icon = dto.Icon,
            MonthlyLimit = dto.MonthlyLimit,
        };
    }

    /// <summary>
    /// Convert the form view model to a modify DTO for adding and updating categories
    /// </summary>
    /// <param name="viewModel">View model from page form</param>
    /// <param name="userId"></param>
    /// <returns>DTO for modifications</returns>
    public static CategoryModifyDto ToModifyDto(this CategoryViewModel viewModel)
    {
        return new CategoryModifyDto
        {
            CategoryId = viewModel.CategoryId ?? Guid.Empty,
            Name = viewModel.Name,
            Description = viewModel.Description,
            Icon = viewModel.Icon,
            MonthlyLimit = viewModel.MonthlyLimit,

        };
    }

    /// <summary>
    /// Convert the entity to a modify DTO for adding and updating categories
    /// </summary>
    /// <param name="model">Model from database</param>
    /// <param name="userId"></param>
    /// <returns>DTO for modifications</returns>
    public static CategoryModifyDto ToModifyDto(this Category entity)
    {
        return new CategoryModifyDto
        {
            CategoryId = entity.CategoryId,
            Name = entity.Name,
            Description = entity.Description,
            Icon = entity.Icon,
            MonthlyLimit = entity.MonthlyLimit
        };
    }

    /// <summary>
    /// Maps a <see cref="Category"/> to a <see cref="CategoryUserListDto"/> used for listing categories owned by a user.
    /// </summary>
    /// <param name="model">Base entity</param>
    /// <param name="validFrom">The last time the entity was edited</param>
    /// <returns></returns>
    public static CategoryUserListDto ToUserListDto(this Category entity)
    {
        return new CategoryUserListDto
        {
            CategoryId = entity.CategoryId,
            Name = entity.Name,
            Description = entity.Description,
            MonthlyLimit = entity.MonthlyLimit,
            Icon = entity.Icon,
            LastModifiedDays = entity.LastModifiedDays ?? 0
        };
    }

    public static CategoryViewModel ToViewModel(this CategoryModifyDto dto)
    {
        return new CategoryViewModel
        {
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            Icon = dto.Icon,
            Name = dto.Name,
            MonthlyLimitDisplay = dto.MonthlyLimit == null ? "$0.00" : dto.MonthlyLimit.Value.ToString("C2", CultureInfo.CurrentCulture),
        };
    }
}
