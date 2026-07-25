using NUnit.Framework;
using PetersPizza.Api.Infrastructure.API.Host.Validators;
using PetersPizza.Api.ViewModels.User;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ValidatorTests;

[TestFixture]
public class LoginUserRequestValidatorTests
{
    private LoginUserRequestValidator _validator;
    
    [SetUp]
    public void Setup()
    {
        _validator = new LoginUserRequestValidator();
    }
    
    [Test]
    public void LoginUserRequest_Valid_ReturnsTrue()
    {
        // Arrange
        var request = new LoginUserRequest { UserName = "test", Password = "test-password" };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBe(true);
        result.Errors.Count.ShouldBe(0);
    }

    [TestCase("", "test-password", "Username is required.")]
    [TestCase(" ", "test-password", "Username is required.")]
    [TestCase("test-username", "", "Password is required.")]
    [TestCase("test-username", " ", "Password is required.")]
    public void LoginUserRequest_Invalid_ReturnsFalse(string userName, string password, string errorMessage)
    {
        // Arrange
        var request = new LoginUserRequest { UserName = userName, Password = password };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe(errorMessage);
    }
}