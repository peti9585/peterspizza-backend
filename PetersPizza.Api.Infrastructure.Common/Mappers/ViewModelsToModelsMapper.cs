using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Infrastructure.Common.Mappers;

public partial class Mapper : MapperBase, IMapper
{
    public RegisterUserRequest Map(ViewModels.User.RegisterUserRequest request)
        => new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Password = request.Password
        };

    public LoginUserRequest Map(ViewModels.User.LoginUserRequest request)
        => new()
        {
            UserName = request.UserName,
            Password = request.Password
        };
}