using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.ViewModels.Pizza;
using PetersPizza.Api.ViewModels.User;

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
}