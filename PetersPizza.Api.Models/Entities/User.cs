using System.ComponentModel.DataAnnotations;

namespace PetersPizza.Api.Models.Entities;

public sealed class User
{
    [Required]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string LastName { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string UserName { get; init; }
    
    [Required]
    [MaxLength(100)]
    public required string Email { get; init; }
    
    [Required]
    [MaxLength(50)]
    public required string PhoneNumber { get; init; }
    
    [Required]
    public required string Password { get; init; }
}