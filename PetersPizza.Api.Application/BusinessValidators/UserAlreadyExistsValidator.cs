using FluentValidation;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Application.BusinessValidators;

public class UserAlreadyExistsValidator : AbstractValidator<(UpdateUserRequest request, int userId)>
{
    public UserAlreadyExistsValidator(IUserRepository userRepository)
    {
        RuleFor(r => r)
            .MustAsync(async (r, _) => await userRepository.AreUserValuesUniqueAsync(r.request.PhoneNumber, r.request.Email, r.userId))
            .WithMessage("The provided values must be unique.");
    }
}