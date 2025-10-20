using FluentValidation;
using PetersPizza.Api.ViewModels.Admin;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class UploadPizzaRequestValidator : AbstractValidator<UploadPizzaRequest>
{
    public UploadPizzaRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(r => r.PizzaName)
            .NotEmpty().WithMessage("Pizza name cannot be empty.");
        
        RuleFor(r => r.Description)
            .NotEmpty().WithMessage("Description cannot be empty.");
        
        RuleFor(r => r.PizzaPrice)
            .GreaterThan(0).WithMessage("Pizza price must be greater than 0.");
        
        RuleFor(r => r.PizzaImage)
            .NotEmpty().WithMessage("Pizza image cannot be empty.");
    }
}