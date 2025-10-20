using PetersPizza.Api.Infrastructure.DataTransferObjects.Pizza;
using PetersPizza.Api.Infrastructure.DataTransferObjects.User;
using PetersPizza.Api.Models.User;
using PetersPizza.Api.Models.Admin;
using PetersPizza.Api.Models.Pizza;

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
    ViewModels.Pizza.GetAllPizzasResponse Map(GetAllPizzasResponse response);
    ViewModels.Pizza.GetPizzasByIdsResponse Map(GetPizzasByIdsResponse response);
    
    // Data Access Objects to Models
    LoginUserInformation Map(DbLoginUserInformation dbLoginUserInformation);
    UserInfo Map(DbUserInfo dbUserInfo);
    GetAllPizzaDetailsResponse Map(IEnumerable<DbPizza> dbPizzas);
    GetPizzasByIdsResponse Map(IEnumerable<DbPizzaById> source);
}