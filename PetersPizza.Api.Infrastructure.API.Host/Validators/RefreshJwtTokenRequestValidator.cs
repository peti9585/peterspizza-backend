using FluentValidation;
using PetersPizza.Api.ViewModels.User;

namespace PetersPizza.Api.Infrastructure.API.Host.Validators;

public class RefreshJwtTokenRequestValidator : AbstractValidator<RefreshJwtTokenRequest>
{
    public RefreshJwtTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}