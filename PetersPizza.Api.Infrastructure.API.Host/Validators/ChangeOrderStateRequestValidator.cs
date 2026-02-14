using FluentValidation;
using PetersPizza.Api.ViewModels.Admin;
using PetersPizza.Api.ViewModels.Common;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class ChangeOrderStateRequestValidator : AbstractValidator<ChangeOrderStateRequest>
{
    public ChangeOrderStateRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(r => r.OrderId)
            .NotEmpty().WithMessage("Order ID is required.");
        
        RuleFor(r => r.NewOrderState)
            .NotEqual(OrderState.Undefined).WithMessage("New order state cannot be undefined.")
            .IsInEnum().WithMessage("New order state is invalid.");
    }
}