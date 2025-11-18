using PetersPizza.Api.Infrastructure.DataTransferObjects.Pizza;
using PetersPizza.Api.Infrastructure.DataTransferObjects.User;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
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
}