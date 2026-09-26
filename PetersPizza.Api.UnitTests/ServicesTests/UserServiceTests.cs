using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using NUnit.Framework;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Application.Services.User;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.User;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ServicesTests;

[TestFixture]
public class UserServiceTests : ServiceTestBase
{
    private IUserRepository _userRepositoryMock;
    private IConfiguration _configurationMock;
    private IPasswordHandlerService<RegisterUserRequest> _passwordHandlerServiceMock;
    private JwtSecurityTokenHandler _jwtTokenHandlerMock;
    private IValidator<(UpdateUserRequest, int)> _updateUserRequestValidatorMock;
    
    private UserService _target;
    
    private const string HashedPassword = "hashed-password";
    private const int RegisteredUserId = 10;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _configurationMock = Substitute.For<IConfiguration>();
        _passwordHandlerServiceMock = Substitute.For<IPasswordHandlerService<RegisterUserRequest>>();
        _jwtTokenHandlerMock = Substitute.For<JwtSecurityTokenHandler>();
        _updateUserRequestValidatorMock = Substitute.For<IValidator<(UpdateUserRequest, int)>>();
        
        _target = new UserService(_userRepositoryMock, _configurationMock, _updateUserRequestValidatorMock, _passwordHandlerServiceMock, _jwtTokenHandlerMock);
    }

    [Test]
    public async Task RegisterUserAsync_Success()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peti8595",
            Email = "peti8595@gmail.com",
            PhoneNumber = "12 34 567890",
            Password = "password-123"
        };
        var expectedRepositoryRequest = new RegisterUserRequest
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Password = HashedPassword
        };
        
        _passwordHandlerServiceMock
            .HashPassword(Arg.Any<RegisterUserRequest>(), Arg.Any<string>())
            .Returns(HashedPassword);
        
        _userRepositoryMock
            .RegisterUserAsync(Arg.Any<RegisterUserRequest>())
            .Returns(RegisteredUserId);

        // Act
        var result = await _target.RegisterUserAsync(request);

        // Assert
        result.ShouldBe(RegisteredUserId);
        
        _passwordHandlerServiceMock
            .Received(1)
            .HashPassword(request, request.Password);
        await _userRepositoryMock
            .Received(1)
            .RegisterUserAsync(Arg.Is<RegisterUserRequest>(p => AssertAreEquivalent(p, expectedRepositoryRequest)));
    }

    [Test]
    public async Task LoginUserAsync_Success()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            UserName = "peti8595",
            Password = "password-123"
        };
        var expectedResponse = new LoginUserResponse
        {
            Name = "Peter Kis",
            JwtToken = "token"
        };
        
        _userRepositoryMock
            .GetUserDetailsAsync(Arg.Any<LoginUserRequest>())
            .Returns(new LoginUserInformation { Name = "Peter Kis", UserId = 10, PasswordHash = "hashed-password" });
        
        _userRepositoryMock
            .UpsertRefreshTokenAsync(Arg.Any<int>(), Arg.Any<Guid>())
            .Returns(Task.CompletedTask);
        
        _passwordHandlerServiceMock
            .IsValidPassword(Arg.Any<string>(), Arg.Any<string>())
            .Returns(true);
        
        _configurationMock.GetSection(Arg.Any<string>()).Value
            .Returns("key");
        
        _jwtTokenHandlerMock.CreateToken(Arg.Any<SecurityTokenDescriptor>())
            .Returns(new JwtSecurityToken());
        _jwtTokenHandlerMock.WriteToken(Arg.Any<JwtSecurityToken>())
            .Returns("token");

        // Act
        var result = await _target.LoginUserAsync(request);

        // Assert
        result.Name.ShouldBe(expectedResponse.Name);
        result.JwtToken.ShouldBe(expectedResponse.JwtToken);
        result.RefreshToken.ShouldBeOfType<Guid>();
        result.RefreshToken.ShouldNotBe(Guid.Empty);

        await _userRepositoryMock
            .Received(1)
            .GetUserDetailsAsync(Arg.Is<LoginUserRequest>(p => AssertAreEquivalent(p, request)));
        
        _passwordHandlerServiceMock
            .Received(1)
            .IsValidPassword(request.Password, "hashed-password");
        
        await _userRepositoryMock
            .Received(1)
            .UpsertRefreshTokenAsync(10, Arg.Any<Guid>());
        
        _jwtTokenHandlerMock
            .Received(1)
            .CreateToken(Arg.Any<SecurityTokenDescriptor>());
        _jwtTokenHandlerMock
            .Received(1)
            .WriteToken(Arg.Any<JwtSecurityToken>());
    }
    
    [Test]
    public async Task LoginUserAsync_InvalidPassword()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            UserName = "peti8595",
            Password = "password-123"
        };
        var expectedResponse = new LoginUserResponse();
        
        _userRepositoryMock
            .GetUserDetailsAsync(Arg.Any<LoginUserRequest>())
            .Returns(new LoginUserInformation { Name = "Peter Kis", UserId = 10, PasswordHash = "hashed-password" });
        
        _passwordHandlerServiceMock
            .IsValidPassword(Arg.Any<string>(), Arg.Any<string>())
            .Returns(false);

        // Act
        var result = await _target.LoginUserAsync(request);

        // Assert
        result.ShouldBeEquivalentTo(expectedResponse);
        
        await _userRepositoryMock
            .Received(1)
            .GetUserDetailsAsync(Arg.Is<LoginUserRequest>(p => AssertAreEquivalent(p, request)));
        
        _passwordHandlerServiceMock
            .Received(1)
            .IsValidPassword(request.Password, "hashed-password");
        
        await _userRepositoryMock
            .DidNotReceive()
            .UpsertRefreshTokenAsync(10, Arg.Any<Guid>());
        
        _jwtTokenHandlerMock
            .DidNotReceive()
            .CreateToken(Arg.Any<SecurityTokenDescriptor>());
        _jwtTokenHandlerMock
            .DidNotReceive()
            .WriteToken(Arg.Any<JwtSecurityToken>());
    }

    [Test]
    public async Task RefreshJwtTokenAsync_Success()
    {
        // Arrange
        var request = new RefreshJwtTokenRequest { RefreshToken = Guid.NewGuid() };
        var expectedJwtToken = "token";

        _userRepositoryMock
            .GetUserByRefreshTokenAsync(Arg.Any<Guid>())
            .Returns(new UserInfo { Id = 1, UserName = "peti8595" });
        _userRepositoryMock
            .UpsertRefreshTokenAsync(Arg.Any<int>(), Arg.Any<Guid>())
            .Returns(Task.CompletedTask);
        
        _configurationMock
            .GetSection(Arg.Any<string>()).Value
            .Returns("key");
        
        _jwtTokenHandlerMock
            .CreateToken(Arg.Any<SecurityTokenDescriptor>())
            .Returns(new JwtSecurityToken());
        _jwtTokenHandlerMock
            .WriteToken(Arg.Any<JwtSecurityToken>())
            .Returns("token");

        // Act
        var result = await _target.RefreshJwtTokenAsync(request);

        // Assert
        result.JwtToken.ShouldBe(expectedJwtToken);
        result.RefreshToken.ShouldBeOfType<Guid>();
        result.RefreshToken.ShouldNotBe(Guid.Empty);
        
        await _userRepositoryMock
            .Received(1)
            .GetUserByRefreshTokenAsync(request.RefreshToken);
        await _userRepositoryMock
            .Received(1)
            .UpsertRefreshTokenAsync(1, Arg.Any<Guid>());
        
        _jwtTokenHandlerMock
            .Received(1)
            .CreateToken(Arg.Any<SecurityTokenDescriptor>());
        _jwtTokenHandlerMock
            .Received(1)
            .WriteToken(Arg.Any<JwtSecurityToken>());
    }

    [Test]
    public async Task RefreshJwtTokenAsync_GetUserInfo_NotFound()
    {
        // Arrange
        var request = new RefreshJwtTokenRequest { RefreshToken = Guid.NewGuid() };
        var expectedResponse = new RefreshJwtTokenResponse();
        
        _userRepositoryMock
            .GetUserByRefreshTokenAsync(Arg.Any<Guid>())
            .Returns(new UserInfo());
        
        // Act
        var result = await _target.RefreshJwtTokenAsync(request);
        
        // Assert
        result.ShouldBeEquivalentTo(expectedResponse);
        
        await _userRepositoryMock
            .Received(1)
            .GetUserByRefreshTokenAsync(request.RefreshToken);
        
        await _userRepositoryMock
            .DidNotReceive()
            .UpsertRefreshTokenAsync(1, Arg.Any<Guid>());
        _jwtTokenHandlerMock
            .DidNotReceive()
            .CreateToken(Arg.Any<SecurityTokenDescriptor>());
        _jwtTokenHandlerMock
            .DidNotReceive()
            .WriteToken(Arg.Any<JwtSecurityToken>());
    }
}