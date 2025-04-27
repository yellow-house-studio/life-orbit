using System.ComponentModel.DataAnnotations;

namespace YellowHouseStudio.LifeOrbit.Application.Family.Common;

public record AddSafeFoodRequest
{
    [Required]
    [MaxLength(100)]
    public string FoodItem { get; init; } = string.Empty;
}