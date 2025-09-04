using FluentValidation;
using PetersPizza.Api.ViewModels.User;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    // TODO: This should be aligned
    public RegisterUserRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(r => r.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.");
        
        RuleFor(r => r.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.");
        
        RuleFor(r => r.UserName)
            .NotEmpty()
            .WithMessage("Username is required.");
        
        RuleFor(r => r.Email)
            .EmailAddress()
            .WithMessage("Invalid email address.");
        
        RuleFor(r => r.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.");
        
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}