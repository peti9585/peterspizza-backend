using PetersPizza.Api.Infrastructure.DataTransferObjects.User;
using PetersPizza.Api.Models.User;
using PetersPizza.Api.Models.Admin;

namespace PetersPizza.Api.Infrastructure.Interfaces.Mappers;

public interface IMapper
{
    // ViewModels to Models
    RegisterUserRequest Map(ViewModels.User.RegisterUserRequest request);
    LoginUserRequest Map(ViewModels.User.LoginUserRequest request);
    RefreshJwtTokenRequest Map(ViewModels.User.RefreshJwtTokenRequest request);
    UploadPizzaRequest Map(ViewModels.Admin.UploadPizzaRequest request);
    
    // Models to ViewModels
    ViewModels.User.LoginUserResponse Map(LoginUserResponse response);
    ViewModels.User.RefreshJwtTokenResponse Map(RefreshJwtTokenResponse response);
    
    // Data Access Objects to Models
    LoginUserInformation Map(DbLoginUserInformation dbLoginUserInformation);
    UserInfo Map(DbUserInfo dbUserInfo);
}