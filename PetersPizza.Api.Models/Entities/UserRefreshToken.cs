using System.ComponentModel.DataAnnotations;

namespace PetersPizza.Api.Models.Entities;

public sealed class UserRefreshToken
{
    [Required]
    public int Id { get; init; }
    
    [Required]
    public required int UserId { get; init; }
    
    // Navigation property
    public User User { get; init; }
    
    [Required]
    public required Guid RefreshToken { get; init; }
    
    [Required]
    public required DateTime ExpirationDate { get; init; }
}