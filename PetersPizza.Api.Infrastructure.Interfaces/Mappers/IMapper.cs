using PetersPizza.Api.Infrastructure.DataTransferObjects.User;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Infrastructure.Interfaces.Mappers;

public interface IMapper
{
    // ViewModels to Models
    RegisterUserRequest Map(ViewModels.User.RegisterUserRequest request);
    
    LoginUserRequest Map(ViewModels.User.LoginUserRequest request);
    
    // Data Access Objects to Models
    LoginUserInformation Map(DbLoginUserInformation dbLoginUserInformation);
}