using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;

namespace PetersPizza.Api.UnitTests.MapperTests;

[TestFixture]
public class ModelsToViewModelsMapperTests : MapperTestBase
{
    [Test]
    public void LoginUserResponse_Model_To_ViewModel_Map_Success()
    {
        // Arrange
        var refreshToken = Guid.NewGuid();

        var model = new Models.User.LoginUserResponse
        {
            Name = "Peter",
            JwtToken = "jwt-token",
            RefreshToken = refreshToken
        };
        var expectedViewModel = new ViewModels.User.LoginUserResponse
        {
            Name = "Peter",
            JwtToken = "jwt-token",
            RefreshToken = refreshToken
        };

        // Act
        var actualViewModel = Mapper.Map(model);

        // Assert
        actualViewModel.ShouldBeEquivalentTo(expectedViewModel);
    }

    [Test]
    public void RefreshJwtTokenResponse_Model_To_ViewModel_Map_Success()
    {
        // Arrange
        var refreshToken = Guid.NewGuid();

        var model = new Models.User.RefreshJwtTokenResponse
        {
            JwtToken = "jwt-token",
            RefreshToken = refreshToken
        };
        var expectedViewModel = new ViewModels.User.RefreshJwtTokenResponse
        {
            JwtToken = "jwt-token",
            RefreshToken = refreshToken
        };

        // Act
        var actualViewModel = Mapper.Map(model);

        // Assert
        actualViewModel.ShouldBeEquivalentTo(expectedViewModel);
    }

    [Test]
    public void GetAllPizzasResponse_Model_To_ViewModel_Map_Success()
    {
        // Arrange
        var model = new Models.Pizza.GetAllPizzasResponse
        {
            GetAllPizzasResponses = new List<Models.Pizza.GetPizzaResponse>
            {
                new()
                {
                    PizzaId = 2,
                    PizzaName = "Margherita",
                    Description = "Italian Pizza",
                    PizzaImageBytes = [1, 2, 3, 4, 5]
                }
            }
        };
        var expectedViewModel = new ViewModels.Pizza.GetAllPizzasResponse
        {
            GetAllPizzasResponses = new List<ViewModels.Pizza.GetPizzaResponse>
            {
                new()
                {
                    PizzaId = 2,
                    PizzaName = "Margherita",
                    Description = "Italian Pizza",
                    PizzaImageBytes = [1, 2, 3, 4, 5]
                }
            }
        };

        // Act
        var actualViewModel = Mapper.Map(model);

        // Assert
        actualViewModel.ShouldBeEquivalentTo(expectedViewModel);
    }

    [Test]
    public void GetPizzasByIdsResponse_Model_To_ViewModel_Map_Success()
    {
        // Arrange
        var model = new Models.Pizza.GetPizzasByIdsResponse
        {
            GetPizzaResponses = new List<Models.Pizza.GetPizzaByIdResponse>
            {
                new()
                {
                    PizzaId = 1,
                    PizzaName = "Pepperoni",
                    PizzaPrice = 10.0m
                }
            }
        };
        var expectedViewModel = new ViewModels.Pizza.GetPizzasByIdsResponse
        {
            GetPizzaResponses = new List<ViewModels.Pizza.GetPizzaByIdResponse>
            {
                new()
                {
                    PizzaId = 1,
                    PizzaName = "Pepperoni",
                    PizzaPrice = 10.0m
                }
            }
        };

        // Act
        var actualViewModel = Mapper.Map(model);

        // Assert
        actualViewModel.ShouldBeEquivalentTo(expectedViewModel);
    }

    [Test]
    public void GetUserDetailsByIdResponse_Model_To_ViewModel_Map_Success()
    {
        // Arrange
        var model = new Models.User.GetUserDetailsByIdResponse
        {
            FirstName = "Peter",
            LastName = "Kis",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890"
        };
        var expectedViewModel = new ViewModels.User.GetUserDetailsByIdResponse
        {
            FirstName = "Peter",
            LastName = "Kis",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890"
        };

        // Act
        var actualViewModel = Mapper.Map(model);

        // Assert
        actualViewModel.ShouldBeEquivalentTo(expectedViewModel);
    }
}