using FluentValidation;
using PetersPizza.Api.ViewModels.Pizza;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class OrderPizzaRequestValidator : AbstractValidator<OrderPizzaRequest>
{
    public OrderPizzaRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(r => r.PizzaId)
            .GreaterThan(0).WithMessage("Pizza ID must be greater than 0.");
        
        RuleFor(r => r.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}