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

    private static GetPizzaResponse Map(Models.Pizza.GetPizzaResponse request)
        => new()
        {
            PizzaName = request.PizzaName,
            Description = request.Description,
            PizzaImageBytes = request.PizzaImageBytes
        };
}