using PetersPizza.Api.Infrastructure.DataTransferObjects.User;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Infrastructure.Common.Mappers;

public partial class Mapper : IMapper
{
    public LoginUserInformation Map(DbLoginUserInformation dbLoginUserInformation)
        => new()
        {
            Name = dbLoginUserInformation.FirstName,
            PasswordHash = dbLoginUserInformation.Password
        };
}