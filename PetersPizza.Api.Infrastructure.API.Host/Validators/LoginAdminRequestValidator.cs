using FluentValidation;
using PetersPizza.Api.ViewModels.Admin;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class LoginAdminRequestValidator : AbstractValidator<LoginAdminRequest>
{
    public LoginAdminRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(r => r.UserName)
            .NotEmpty().WithMessage("Username is required.");
        
        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}