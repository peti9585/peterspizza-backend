using PetersPizza.Api.Infrastructure.DataAccessObjects.Admin;
using PetersPizza.Api.Infrastructure.DataAccessObjects.Pizza;
using PetersPizza.Api.Infrastructure.DataAccessObjects.User;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.Models.Admin;
using PetersPizza.Api.Models.Common;
using PetersPizza.Api.Models.Pizza;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Infrastructure.Common.Mappers;

public partial class Mapper : IMapper
{
    public LoginUserInformation Map(DbLoginUserInformation dbLoginUserInformation)
        => new()
        {
            UserId = dbLoginUserInformation.Id,
            Name = dbLoginUserInformation.FirstName,
            PasswordHash = dbLoginUserInformation.Password
        };

    public UserInfo Map(DbUserInfo dbUserInfo)
        => new()
        {
            Id = dbUserInfo.Id,
            UserName = dbUserInfo.UserName
        };

    public GetAllPizzaDetailsResponse Map(IEnumerable<DbPizza> dbPizzas)
        => new() { GetAllPizzaDetailResponses = MapEnumerable(dbPizzas, Map) };

    public GetPizzasByIdsResponse Map(IEnumerable<DbPizzaById> source)
        => new() { GetPizzaResponses = MapEnumerable(source, Map) };
    
    public GetUserDetailsByIdResponse Map(DbGetUserDetailsByIdResponse source)
        => new()
        {
            FirstName = source.FirstName,
            LastName = source.LastName,
            PhoneNumber = source.PhoneNumber,
            Email = source.Email
        };

    public Models.Pizza.GetAllOrdersResponse Map(IEnumerable<DbGetAllOrders> source)
        => new()
        {
            GetAllOrderResponses = MapEnumerable(source, Map)
        };

    public LoginAdminInformation Map(DbLoginAdminInformation source)
        => new()
        {
            AdminId = source.Id,
            Name = source.Name,
            PasswordHash = source.Password
        };

    public IEnumerable<GetAllOrdersRawResponse> Map(IEnumerable<DbGetAllOrdersForToday> source)
        => MapEnumerable(source, Map);

    private static GetAllPizzaDetailResponse Map(DbPizza dbPizza)
        => new()
        {
            PizzaId = dbPizza.Id,
            PizzaName = dbPizza.Name,
            Description = dbPizza.Description,
            PizzaImageId = dbPizza.ImageId
        };
    
    private static GetPizzaByIdResponse Map(DbPizzaById source)
        => new()
        {
            PizzaId = source.Id,
            PizzaName = source.Name,
            PizzaPrice = source.Price
        };
    
    private static Models.Pizza.GetAllOrderResponse Map(DbGetAllOrders source)
        => new()
        {
            OrderId = source.OrderId,
            OrderState = (OrderState)source.OrderState,
            OrderDate = source.OrderDate
        };

    private static GetAllOrdersRawResponse Map(DbGetAllOrdersForToday source)
        => new()
        {
            OrderIdInteger = source.Id,
            OrderIdGuid = source.OrderId,
            UserName = source.UserName,
            OrderState = (OrderState)source.OrderState,
            PizzaName = source.PizzaName,
            Price = source.Price,
            Quantity = source.Count,
            OrderDate = source.OrderDate
        };
}