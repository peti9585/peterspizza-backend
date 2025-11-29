using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.Models.Admin;
using PetersPizza.Api.Models.Pizza;
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

    public RefreshJwtTokenRequest Map(ViewModels.User.RefreshJwtTokenRequest request)
        => new()
        {
            RefreshToken = request.RefreshToken
        };

    public UploadPizzaRequest Map(ViewModels.Admin.UploadPizzaRequest request)
        => new()
        {
            PizzaName = request.PizzaName,
            Description = request.Description,
            PizzaPrice = request.PizzaPrice,
            PizzaImage = request.PizzaImage
        };

    public OrderPizzasRequest Map(ViewModels.Pizza.OrderPizzasRequest request)
        => new()
        {
            UserId = request.UserId,
            OrderId = request.OrderId,
            OrderPizzaRequests = MapEnumerable(request.OrderPizzaRequests, Map)
        };

    public UpdateUserRequest Map(ViewModels.User.UpdateUserRequest request)
        => new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        };

    private static OrderPizzaRequest Map(ViewModels.Pizza.OrderPizzaRequest request)
        => new()
        {
            PizzaId = request.PizzaId,
            Quantity = request.Quantity
        };
}