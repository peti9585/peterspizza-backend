using NUnit.Framework;
using PetersPizza.Api.Infrastructure.API.Host.Validators;
using PetersPizza.Api.ViewModels.Pizza;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ValidatorTests;

[TestFixture]
public class OrderPizzaRequestValidatorTests
{
    private OrderPizzaRequestValidator _validator;
    
    [SetUp]
    public void Setup()
    {
        _validator = new OrderPizzaRequestValidator();
    }

    [Test]
    public void OrderPizzaRequest_Valid_ReturnsTrue()
    {
        // Arrange
        var request = new OrderPizzaRequest { PizzaId = 1, Quantity = 1 };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(true);
        result.Errors.Count.ShouldBe(0);
    }

    [TestCase(0, 1, "Pizza ID must be greater than 0.")]
    [TestCase(1, 0, "Quantity must be greater than 0.")]
    public void OrderPizzaRequest_Invalid_ReturnsFalse(int pizzaId, int quantity, string errorMessage)
    {
        // Arrange
        var request = new OrderPizzaRequest { PizzaId = pizzaId, Quantity = quantity };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(errorMessage);
    }
}