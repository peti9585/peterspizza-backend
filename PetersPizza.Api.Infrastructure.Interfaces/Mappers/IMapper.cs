using PetersPizza.Api.Infrastructure.DataAccessObjects.Admin;
using PetersPizza.Api.Infrastructure.DataAccessObjects.Pizza;
using PetersPizza.Api.Infrastructure.DataAccessObjects.User;
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
    OrderPizzasRequest Map(ViewModels.Pizza.OrderPizzasRequest request);
    UpdateUserRequest Map(ViewModels.User.UpdateUserRequest request);
    LoginAdminRequest Map(ViewModels.Admin.LoginAdminRequest request);
    JwtTokenInformationRequest Map(ViewModels.Admin.JwtTokenInformationRequest request);
    ChangeOrderStateRequest Map(ViewModels.Admin.ChangeOrderStateRequest request);
    
    // Models to ViewModels
    ViewModels.User.LoginUserResponse Map(LoginUserResponse response);
    ViewModels.User.RefreshJwtTokenResponse Map(RefreshJwtTokenResponse response);
    ViewModels.Pizza.GetAllPizzasResponse Map(GetAllPizzasResponse response);
    ViewModels.Pizza.GetPizzasByIdsResponse Map(GetPizzasByIdsResponse response);
    ViewModels.User.GetUserDetailsByIdResponse Map(GetUserDetailsByIdResponse response);
    ViewModels.Pizza.GetAllOrdersResponse Map(Models.Pizza.GetAllOrdersResponse response);
    ViewModels.Admin.LoginAdminResponse Map(LoginAdminResponse response);
    ViewModels.Admin.JwtTokenInformationResponse Map(JwtTokenInformationResponse response);
    ViewModels.Admin.GetAllOrdersResponse Map(Models.Admin.GetAllOrdersResponse source);
    
    // Data Access Objects to Models
    LoginUserInformation Map(DbLoginUserInformation source);
    UserInfo Map(DbUserInfo source);
    GetAllPizzaDetailsResponse Map(IEnumerable<DbPizza> source);
    GetPizzasByIdsResponse Map(IEnumerable<DbPizzaById> source);
    GetUserDetailsByIdResponse Map(DbGetUserDetailsByIdResponse source);
    Models.Pizza.GetAllOrdersResponse Map(IEnumerable<DbGetAllOrders> source);
    LoginAdminInformation Map(DbLoginAdminInformation source);
    IEnumerable<GetAllOrdersRawResponse> Map(IEnumerable<DbGetAllOrdersForToday> source);
}