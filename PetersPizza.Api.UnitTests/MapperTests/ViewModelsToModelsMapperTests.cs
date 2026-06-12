using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;

namespace PetersPizza.Api.UnitTests.MapperTests;

[TestFixture]
public class ViewModelsToModelsMapperTests : MapperTestBase
{
    [Test]
    public void RegisterUserRequest_ViewModel_To_Model_Map_Success()
    {
        // Arrange
        var viewModel = new ViewModels.User.RegisterUserRequest
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peti8595",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890",
            Password = "password-123"
        };
        var expectedModel = new Models.User.RegisterUserRequest
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peti8595",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890",
            Password = "password-123"
        };

        // Act
        var actualModel = Mapper.Map(viewModel);

        // Assert
        actualModel.ShouldBeEquivalentTo(expectedModel);
    }

    [Test]
    public void LoginUserRequest_ViewModel_To_Model_Map_Success()
    {
        // Arrange
        var viewModel = new ViewModels.User.LoginUserRequest
        {
            UserName = "peti8595",
            Password = "password-123"
        };
        var expectedModel = new Models.User.LoginUserRequest
        {
            UserName = "peti8595",
            Password = "password-123"
        };

        // Act
        var actualModel = Mapper.Map(viewModel);

        // Assert
        actualModel.ShouldBeEquivalentTo(expectedModel);
    }

    [Test]
    public void RefreshJwtTokenRequest_ViewModel_To_Model_Map_Success()
    {
        // Arrange
        var refreshToken = Guid.NewGuid();
        
        var viewModel = new ViewModels.User.RefreshJwtTokenRequest
        {
            RefreshToken = refreshToken
        };
        var expectedModel = new Models.User.RefreshJwtTokenRequest
        {
            RefreshToken = refreshToken
        };

        // Act
        var actualModel = Mapper.Map(viewModel);

        // Assert
        actualModel.ShouldBeEquivalentTo(expectedModel);
    }

    [Test]
    public void UploadPizzaRequest_ViewModel_To_Model_Map_Success()
    {
        // Arrange
        var formFile = TestData.GetFormFile();
        
        var viewModel = new ViewModels.Admin.UploadPizzaRequest
        {
            PizzaName = "Margherita",
            PizzaPrice = 5.0m,
            Description = "Italian Pizza",
            PizzaImage = formFile
        };

        var expectedModel = new Models.Admin.UploadPizzaRequest
        {
            PizzaName = "Margherita",
            PizzaPrice = 5.0m,
            Description = "Italian Pizza",
            PizzaImage = formFile
        };

        // Act
        var actualModel = Mapper.Map(viewModel);

        // Assert
        actualModel.ShouldBeEquivalentTo(expectedModel);
    }

    [Test]
    public void OrderPizzasRequest_ViewModel_To_Model_Map_Success()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var viewModel = new ViewModels.Pizza.OrderPizzasRequest
        {
            UserId = 1,
            OrderId = orderId,
            OrderPizzaRequests = new List<ViewModels.Pizza.OrderPizzaRequest>
            {
                new() { PizzaId = 1, Quantity = 1 }
            }
        };
        var expectedModel = new Models.Pizza.OrderPizzasRequest
        {
            UserId = 1,
            OrderId = orderId,
            OrderPizzaRequests = new List<Models.Pizza.OrderPizzaRequest>
            {
                new() { PizzaId = 1, Quantity = 1 }
            }
        };

        // Act
        var actualModel = Mapper.Map(viewModel);

        // Assert
        actualModel.ShouldBeEquivalentTo(expectedModel);
    }
}