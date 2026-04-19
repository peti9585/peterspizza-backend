using FluentValidation;
using PetersPizza.Api.ViewModels.Admin;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class JwtTokenInformationRequestValidator : AbstractValidator<JwtTokenInformationRequest>
{
    public JwtTokenInformationRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(r => r.JwtToken)
            .NotEmpty()
            .WithMessage("Jwt token is required.");
    }
}