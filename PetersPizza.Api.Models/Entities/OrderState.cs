using System.ComponentModel.DataAnnotations;

namespace PetersPizza.Api.Models.Entities;

public sealed class OrderState
{
    [Required]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string Name { get; init; }
    
    [Required]
    public required DateTime CreatedAt { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string CreatedBy { get; init; }
}