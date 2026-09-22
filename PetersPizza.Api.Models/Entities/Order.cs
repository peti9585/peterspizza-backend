using System.ComponentModel.DataAnnotations;

namespace PetersPizza.Api.Models.Entities;

public sealed class Order
{
    [Required]
    public int Id { get; init; }
    
    [Required]
    public required Guid OrderId { get; init; }
    
    [Required]
    public required int UserId { get; init; }
    
    // Navigation property
    public User User { get; init; }
    
    [Required]
    public required int PizzaId { get; init; }
    
    // Navigation property
    public Pizza Pizza { get; init; }
    
    [Required]
    public required int Count { get; init; }
    
    [Required]
    public required int OrderStateId { get; init; }
    
    // Navigation property
    public OrderState OrderState { get; init; }
    
    [Required]
    public required DateTime OrderDate { get; init; }
}