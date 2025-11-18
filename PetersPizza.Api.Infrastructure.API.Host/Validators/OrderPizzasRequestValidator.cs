using FluentValidation;
using PetersPizza.Api.ViewModels.Pizza;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class OrderPizzasRequestValidator : AbstractValidator<OrderPizzasRequest>
{
    public OrderPizzasRequestValidator(IValidator<OrderPizzaRequest> validatorInner)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(r => r.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleForEach(r => r.OrderPizzaRequests)
            .SetValidator(validatorInner);
    }
}