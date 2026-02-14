using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.ViewModels.Admin;
using PetersPizza.Api.ViewModels.Common;
using PetersPizza.Api.ViewModels.Pizza;
using PetersPizza.Api.ViewModels.User;
using GetAllOrderResponse = PetersPizza.Api.ViewModels.Admin.GetAllOrderResponse;

namespace PetersPizza.Api.Infrastructure.Common.Mappers;

public partial class Mapper : IMapper
{
    public LoginUserResponse Map(Models.User.LoginUserResponse response)
        => new()
        {
            Name = response.Name,
            JwtToken = response.JwtToken,
            RefreshToken = response.RefreshToken
        };

    public RefreshJwtTokenResponse Map(Models.User.RefreshJwtTokenResponse request)
        => new()
        {
            JwtToken = request.JwtToken,
            RefreshToken = request.RefreshToken
        };
    
    public GetAllPizzasResponse Map(Models.Pizza.GetAllPizzasResponse request)
        => new()
        {
            GetAllPizzasResponses = MapEnumerable(request.GetAllPizzasResponses, Map)
        };

    public GetPizzasByIdsResponse Map(Models.Pizza.GetPizzasByIdsResponse source)
        => new()
        {
            GetPizzaResponses = MapEnumerable(source.GetPizzaResponses, Map)
        };

    public GetUserDetailsByIdResponse Map(Models.User.GetUserDetailsByIdResponse source)
        => new()
        {
            FirstName = source.FirstName,
            LastName = source.LastName,
            PhoneNumber = source.PhoneNumber,
            Email = source.Email
        };

    public ViewModels.Pizza.GetAllOrdersResponse Map(Models.Pizza.GetAllOrdersResponse response)
        => new()
        {
            GetAllOrderResponses = MapEnumerable(response.GetAllOrderResponses, Map)
        };

    public LoginAdminResponse Map(Models.Admin.LoginAdminResponse response)
        => new()
        {
            Name = response.Name,
            JwtToken = response.JwtToken
        };

    public JwtTokenInformationResponse Map(Models.Admin.JwtTokenInformationResponse request)
        => new()
        {
            IsAdmin = request.IsAdmin
        };

    public ViewModels.Admin.GetAllOrdersResponse Map(Models.Admin.GetAllOrdersResponse source)
        => new()
        {
            GetAllOrderResponses = MapEnumerable(source.GetAllOrderResponses, Map)
        };

    private static GetPizzaResponse Map(Models.Pizza.GetPizzaResponse request)
        => new()
        {
            PizzaId = request.PizzaId,
            PizzaName = request.PizzaName,
            Description = request.Description,
            PizzaImageBytes = request.PizzaImageBytes
        };
    
    private static GetPizzaByIdResponse Map(Models.Pizza.GetPizzaByIdResponse source)
        => new()
        {
            PizzaId = source.PizzaId,
            PizzaName = source.PizzaName,
            PizzaPrice = source.PizzaPrice
        };

    private static ViewModels.Pizza.GetAllOrderResponse Map(Models.Pizza.GetAllOrderResponse source)
        => new()
        {
            OrderId = source.OrderId,
            OrderState = (OrderState)source.OrderState,
            OrderDate = source.OrderDate
        };

    private static GetAllOrderResponse Map(Models.Admin.GetAllOrderResponse source)
        => new()
        {
            OrderId = source.OrderId,
            UserName = source.UserName,
            OrderState = (OrderState)source.OrderState,
            OrderDate = source.OrderDate,
            OrderItems = MapEnumerable(source.OrderItems, Map)
        };

    private static OrderItem Map(Models.Admin.OrderItem source)
        => new()
        {
            OrderId = source.OrderId,
            PizzaName = source.PizzaName,
            Quantity = source.Quantity,
            Price = source.Price
        };
}