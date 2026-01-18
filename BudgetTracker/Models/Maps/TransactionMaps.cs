using BudgetTracker.Data.Entities;
using BudgetTracker.Models.DTOs;
using BudgetTracker.Models.ViewModels;
using System.Globalization;

namespace BudgetTracker.Models.Maps;

/// <summary>
/// Maps for transaction models
/// </summary>
public static class TransactionMaps
{

    public static Transaction ToEntity(this TransactionModifyDto dto)
    {
        return new Transaction()
        {
            TransactionId = dto.TransactionId,
            Amount = dto.Amount,
            TransactionType = dto.TransactionType,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            DateOccurred = dto.DateOccurred,
            PlaceOfPurchase = dto.PlaceOfPurchase
        };
    }

    public static TransactionModifyDto ToModifyDto(this Transaction entity)
    {
        return new TransactionModifyDto()
        {
            TransactionId = entity.TransactionId,
            Amount = entity.Amount,
            TransactionType = entity.TransactionType,
            CategoryId = entity.CategoryId,
            Description = entity.Description,
            DateOccurred = entity.DateOccurred,
            PlaceOfPurchase = entity.PlaceOfPurchase
        };
    }

    public static TransactionModifyDto ToModifyDto(this TransactionViewModel viewModel)
    {
        return new TransactionModifyDto()
        {
            TransactionId = viewModel.TransactionId ?? Guid.Empty,
            Amount = viewModel.Amount,
            TransactionType = viewModel.TransactionType,
            CategoryId = viewModel.CategoryId,
            Description = viewModel.Description!,
            DateOccurred = viewModel.DateOccurred ?? default,
            PlaceOfPurchase = viewModel.PlaceOfPurchase
        };
    }

    public static TransactionUserListItemDto ToUserListDto(this Transaction entity)
    {
        return new TransactionUserListItemDto()
        {
            TransactionId = entity.TransactionId,
            Amount = entity.Amount,
            TransactionType = entity.TransactionType,
            Category = entity.Category?.Name,
            DateOccurred = entity.DateOccurred,
            Description = entity.Description,
            PlaceOfPurchase = entity.PlaceOfPurchase
        };
    }

    public static TransactionViewModel ToViewModel(this TransactionModifyDto dto)
    {
        return new TransactionViewModel()
        {
            TransactionId = dto.TransactionId,
            TransactionType = dto.TransactionType,
            CategoryId = dto.CategoryId,
            DateOccurred = dto.DateOccurred,
            Description = dto.Description,
            PlaceOfPurchase = dto.PlaceOfPurchase,
            AmountDisplay = dto.Amount.ToString("C2", CultureInfo.CurrentCulture)
        };
    }
}