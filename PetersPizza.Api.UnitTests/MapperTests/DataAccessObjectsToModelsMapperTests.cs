using System;
using System.Collections.Generic;
using NUnit.Framework;
using PetersPizza.Api.Infrastructure.DataAccessObjects.Pizza;
using PetersPizza.Api.Infrastructure.DataAccessObjects.User;
using PetersPizza.Api.Models.Pizza;
using PetersPizza.Api.Models.User;
using Shouldly;

namespace PetersPizza.Api.UnitTests.MapperTests;

[TestFixture]
public class DataAccessObjectsToModelsMapperTests : MapperTestBase
{
    [Test]
    public void DbLoginUserInformation_To_LoginUserInformation_Map_Success()
    {
        // Arrange
        var dbResponse = new DbLoginUserInformation
        {
            Id = 1,
            FirstName = "Peter",
            Password = "password-123"
        };
        var expectedModel = new LoginUserInformation
        {
            UserId = 1,
            Name = "Peter",
            PasswordHash = "password-123"
        };

        // Act
        var actualResult = Mapper.Map(dbResponse);

        // Assert
        actualResult.ShouldBeEquivalentTo(expectedModel);
    }

    [Test]
    public void DbUserInfo_To_UserInfo_Map_Success()
    {
        // Arrange
        var dbResponse = new DbUserInfo
        {
            Id = 1,
            UserName = "peti8595"
        };
        var expectedModel = new UserInfo
        {
            Id = 1,
            UserName = "peti8595"
        };

        // Act
        var actualResult = Mapper.Map(dbResponse);

        // Assert
        actualResult.ShouldBeEquivalentTo(expectedModel);
    }

    [Test]
    public void DbPizzaCollection_To_GetAllPizzaDetailsResponse_Map_Success()
    {
        // Arrange
        var imageId = Guid.NewGuid();
        
        var dbResponse = new List<DbPizza>
        {
            new()
            {
                Id = 2,
                Name = "Margherita",
                Description = "Italian Pizza",
                ImageId = imageId
            }
        };
        var expectedModel = new GetAllPizzaDetailsResponse
        {
            GetAllPizzaDetailResponses = new List<GetAllPizzaDetailResponse>
            {
                new()
                {
                    PizzaId = 2,
                    PizzaName = "Margherita",
                    Description = "Italian Pizza",
                    PizzaImageId = imageId
                }
            }
        };

        // Act
        var actualResult = Mapper.Map(dbResponse);

        // Assert
        actualResult.ShouldBeEquivalentTo(expectedModel);
    }

    [Test]
    public void DbPizzaByIdCollection_To_GetPizzasByIdsResponse_Map_Success()
    {
        // Arrange
        var dbResponse = new List<DbPizzaById>
        {
            new()
            {
                Id = 2,
                Name = "Margherita",
                Price = 5.0m
            }
        };
        var expectedModel = new GetPizzasByIdsResponse
        {
            GetPizzaResponses = new List<GetPizzaByIdResponse>
            {
                new()
                {
                    PizzaId = 2,
                    PizzaName = "Margherita",
                    PizzaPrice = 5.0m
                }
            }
        };

        // Act
        var actualResult = Mapper.Map(dbResponse);

        // Assert
        actualResult.ShouldBeEquivalentTo(expectedModel);
    }

    [Test]
    public void DbGetUserDetailsByIdResponse_To_GetUserDetailsByIdResponse_Map_Success()
    {
        // Arrange
        var dbResponse = new DbGetUserDetailsByIdResponse
        {
            FirstName = "Peter",
            LastName = "Kis",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890"
        };
        var expectedModel = new GetUserDetailsByIdResponse
        {
            FirstName = "Peter",
            LastName = "Kis",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890"
        };

        // Act
        var actualResult = Mapper.Map(dbResponse);

        // Assert
        actualResult.ShouldBeEquivalentTo(expectedModel);
    }
}