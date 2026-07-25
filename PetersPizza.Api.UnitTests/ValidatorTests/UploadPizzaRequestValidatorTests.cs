using NUnit.Framework;
using PetersPizza.Api.Infrastructure.API.Host.Validators;
using PetersPizza.Api.ViewModels.Admin;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ValidatorTests;

[TestFixture]
public class UploadPizzaRequestValidatorTests
{
    private UploadPizzaRequestValidator _validator;
    
    [SetUp]
    public void Setup() => _validator = new UploadPizzaRequestValidator();

    [Test]
    public void UploadPizzaRequest_Valid_ReturnsTrue()
    {
        // Arrange
        var request = new UploadPizzaRequest
        {
            PizzaName = "Test Pizza",
            PizzaPrice = 10.5m,
            Description = "Test Description",
            PizzaImage = TestData.GetFormFile()
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBe(true);
        result.Errors.Count.ShouldBe(0);
    }

    [Test]
    public void UploadPizzaRequest_PizzaName_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new UploadPizzaRequest
        {
            PizzaName = "",
            PizzaPrice = 10.5m,
            Description = "Test Description",
            PizzaImage = TestData.GetFormFile()
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Pizza name cannot be empty.");
    }

    [Test]
    public void UploadPizzaRequest_PizzaPrice_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new UploadPizzaRequest
        {
            PizzaName = "Test Pizza",
            PizzaPrice = 0,
            Description = "Test Description",
            PizzaImage = TestData.GetFormFile()
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Pizza price must be greater than 0.");
    }

    [Test]
    public void UploadPizzaRequest_Description_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new UploadPizzaRequest
        {
            PizzaName = "Test Pizza",
            PizzaPrice = 10.5m,
            Description = "",
            PizzaImage = TestData.GetFormFile()
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Description cannot be empty.");
    }

    [Test]
    public void UploadPizzaRequest_PizzaImage_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new UploadPizzaRequest
        {
            PizzaName = "Test Pizza",
            PizzaPrice = 10.5m,
            Description = "Test Description",
            PizzaImage = null
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Pizza image cannot be empty.");
    }
}