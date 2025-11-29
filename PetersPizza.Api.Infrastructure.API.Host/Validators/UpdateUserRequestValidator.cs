using FluentValidation;
using PetersPizza.Api.ViewModels.User;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(r => r.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty.")
            .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.");
        
        RuleFor(r => r.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty.")
            .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters.");
        
        RuleFor(r => r.PhoneNumber)
            .NotEmpty().WithMessage("Phone number cannot be empty.")
            .MaximumLength(50).WithMessage("Phone number cannot be longer than 50 characters.");
        
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email address.")
            .MaximumLength(100).WithMessage("Email cannot be longer than 100 characters.");
    }
}