using System;
using NUnit.Framework;
using PetersPizza.Api.Infrastructure.API.Host.Validators;
using PetersPizza.Api.ViewModels.User;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ValidatorTests;

[TestFixture]
public class RefreshJwtTokenRequestValidatorTests
{
    private RefreshJwtTokenRequestValidator _validator;
    
    [SetUp]
    public void Setup()
    {
        _validator = new RefreshJwtTokenRequestValidator();
    }

    [Test]
    public void RefreshJwtTokenRequest_Valid_ReturnsTrue()
    {
        // Arrange
        var request = new RefreshJwtTokenRequest { RefreshToken = Guid.NewGuid() };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBe(true);
        result.Errors.Count.ShouldBe(0);
    }

    [Test]
    public void RefreshJwtTokenRequest_Invalid_ReturnsFalse()
    {
        // Arrange
        var request = new RefreshJwtTokenRequest { RefreshToken = Guid.Empty };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorMessage.ShouldBe("Refresh token is required.");
    }
}