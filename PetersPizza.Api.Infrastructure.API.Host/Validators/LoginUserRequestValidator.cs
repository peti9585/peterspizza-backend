using FluentValidation;
using PetersPizza.Api.ViewModels.User;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    // TODO: This should be aligned
    public LoginUserRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(r => r.UserName)
            .NotEmpty()
            .WithMessage("Username is required.");
        
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}