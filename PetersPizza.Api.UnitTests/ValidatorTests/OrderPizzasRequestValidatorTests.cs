using System;
using NUnit.Framework;
using PetersPizza.Api.Infrastructure.API.Host.Validators;
using PetersPizza.Api.ViewModels.Pizza;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ValidatorTests;

[TestFixture]
public class OrderPizzasRequestValidatorTests
{
    private OrderPizzasRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new OrderPizzasRequestValidator(new OrderPizzaRequestValidator());
    }

    [Test]
    public void OrderPizzasRequest_Valid_ReturnsTrue()
    {
        // Arrange
        var request = new OrderPizzasRequest
        {
            OrderId = Guid.NewGuid(),
            UserId = 1,
            OrderPizzaRequests = [new OrderPizzaRequest { PizzaId = 1, Quantity = 1 }]
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBe(true);
        result.Errors.Count.ShouldBe(0);
    }

    [Test]
    public void OrderPizzasRequest_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new OrderPizzasRequest
        {
            OrderId = Guid.NewGuid(),
            UserId = 0,
            OrderPizzaRequests = [new OrderPizzaRequest { PizzaId = 1, Quantity = 1 }]
        };
        
        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("User ID must be greater than 0.");
    }
}