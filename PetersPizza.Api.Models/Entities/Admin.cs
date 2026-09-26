using System.ComponentModel.DataAnnotations;

namespace PetersPizza.Api.Models.Entities;

public sealed class Admin
{
    [Required]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string Name { get; init; }
    
    [Required]
    public required string Password { get; init; }
}