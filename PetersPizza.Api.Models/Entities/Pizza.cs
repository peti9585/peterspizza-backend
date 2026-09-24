using System.ComponentModel.DataAnnotations;

namespace PetersPizza.Api.Models.Entities;

public sealed class Pizza
{
    public int Id { get; init; }
    
    [Required]
    public required Guid ImageId { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string Name { get; init; }
    
    [Required]
    public required string Description { get; init; }
    
    [Required]
    public required decimal Price { get; init; }
}