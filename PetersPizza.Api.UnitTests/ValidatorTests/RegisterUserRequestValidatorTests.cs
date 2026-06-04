using NUnit.Framework;
using PetersPizza.Api.Infrastructure.API.Host.Validators;
using PetersPizza.Api.ViewModels.User;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ValidatorTests;

[TestFixture]
public class RegisterUserRequestValidatorTests
{
    private RegisterUserRequestValidator _validator;
    
    [SetUp]
    public void Setup()
    {
        _validator = new RegisterUserRequestValidator();
    }

    [Test]
    public void RegisterUserRequest_Valid_ReturnsTrue()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peti8595",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890",
            Password = "test-password123"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBe(true);
        result.Errors.Count.ShouldBe(0);
    }

    [Test]
    public void RegisterUserRequest_FirstName_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "",
            LastName = "Kis",
            UserName = "peti8595",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890",
            Password = "test-password123"
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("First name is required.");
    }

    [Test]
    public void RegisterUserRequest_LastName_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Peter",
            LastName = "",
            UserName = "peti8595",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890",
            Password = "test-password123"
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Last name is required.");
    }

    [Test]
    public void RegisterUserRequest_UserName_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890",
            Password = "test-password123"
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Username is required.");
    }

    [Test]
    public void RegisterUserRequest_Email_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peti8595",
            Email = "peti8595gmail.com",
            PhoneNumber = "12 34 567890",
            Password = "test-password123"
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Invalid email address.");
    }

    [Test]
    public void RegisterUserRequest_PhoneNumber_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peti8595",
            Email = "peti8595@gmail.com",
            PhoneNumber = "",
            Password = "test-password123"
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Phone number is required.");
    }

    [Test]
    public void RegisterUserRequest_Password_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peti8595",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890",
            Password = ""
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Password is required.");
    }
}