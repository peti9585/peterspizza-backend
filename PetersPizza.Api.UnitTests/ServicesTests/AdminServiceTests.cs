using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using NUnit.Framework;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Application.Interfaces.SignalR;
using PetersPizza.Api.Application.Services.Admin;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;
using PetersPizza.Api.Models.Common;
using PetersPizza.Api.Models.SignalR;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ServicesTests;

[TestFixture]
public class AdminServiceTests : ServiceTestBase
{
    private IImageHandlerService _imageHandlerServiceMock;
    private IAdminRepository _adminRepositoryMock;
    private IConfiguration _configurationMock;
    private IPasswordHandlerService<LoginAdminRequest> _passwordHandlerServiceMock;
    private JwtSecurityTokenHandler _jwtTokenHandlerMock;
    private IUserOrdersHub _hubContextMock;
    
    private AdminService _target;

    [SetUp]
    public void Setup()
    {
        _imageHandlerServiceMock = Substitute.For<IImageHandlerService>();
        _adminRepositoryMock = Substitute.For<IAdminRepository>();
        _configurationMock = Substitute.For<IConfiguration>();
        _passwordHandlerServiceMock = Substitute.For<IPasswordHandlerService<LoginAdminRequest>>();
        _jwtTokenHandlerMock = Substitute.For<JwtSecurityTokenHandler>();
        _hubContextMock = Substitute.For<IUserOrdersHub>();
        
        _target = new AdminService(_adminRepositoryMock, _configurationMock, _imageHandlerServiceMock, _passwordHandlerServiceMock, _jwtTokenHandlerMock, _hubContextMock);
    }

    [Test]
    public async Task UploadPizzaAsync_InsertsPizzaWithUploadedImageId()
    {
        // Arrange
        var request = new UploadPizzaRequest
        {
            PizzaName = "Margherita",
            Description = "Italian Pizza",
            PizzaPrice = 5.0m,
            PizzaImage = TestData.GetFormFile()
        };

        _imageHandlerServiceMock.EnsureImagesFolderExistsAndReturnPath()
            .Returns("C:/");
        _imageHandlerServiceMock.CompressAndSaveAsync(Arg.Any<IFormFile>(), Arg.Any<string>(), Arg.Any<int>())
            .Returns(Task.CompletedTask);
        _adminRepositoryMock.InsertPizzaAsync(Arg.Any<InsertPizzaRequest>())
            .Returns(Task.CompletedTask);

        // Act
        await _target.UploadPizzaAsync(request);

        // Assert
        await _adminRepositoryMock.Received(1)
            .InsertPizzaAsync(Arg.Is<InsertPizzaRequest>(insertRequest =>
                insertRequest.PizzaName == request.PizzaName &&
                insertRequest.Description == request.Description &&
                insertRequest.PizzaPrice == request.PizzaPrice &&
                insertRequest.PizzaImageId != Guid.Empty));
    }

    [Test]
    public async Task UploadPizzaAsync_WhenInsertPizzaAsync_ThrowsException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Insert failed.");
        var request = new UploadPizzaRequest
        {
            PizzaName = "Margherita",
            Description = "Italian Pizza",
            PizzaPrice = 5.0m,
            PizzaImage = TestData.GetFormFile()
        };
        _adminRepositoryMock.InsertPizzaAsync(Arg.Any<InsertPizzaRequest>())
            .Returns(Task.FromException(expectedException));

        // Act
        var actualException = Assert.ThrowsAsync<InvalidOperationException>(async () => await _target.UploadPizzaAsync(request));

        // Assert
        actualException.ShouldBe(expectedException);
    }

    [Test]
    public async Task LoginAdminAsync_Success()
    {
        // Arrange
        var request = new LoginAdminRequest
        {
            UserName = "admin",
            Password = "adminadmin"
        };

        var expectedResponse = new LoginAdminResponse
        {
            Name = "Peter",
            JwtToken = "token"
        };

        _adminRepositoryMock
            .GetAdminDetailsAsync(Arg.Any<LoginAdminRequest>())
            .Returns(new LoginAdminInformation
            {
                AdminId = 2,
                Name = "Peter",
                PasswordHash = "hashed-password"
            });
        
        _passwordHandlerServiceMock
            .IsValidPassword(Arg.Any<string>(), Arg.Any<string>())
            .Returns(true);

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
        var result = await _target.LoginAdminAsync(request);

        // Assert
        result.ShouldBeEquivalentTo(expectedResponse);
        
        await _adminRepositoryMock
            .Received(1)
            .GetAdminDetailsAsync(Arg.Is<LoginAdminRequest>(r => AssertAreEquivalent(r, request)));
        
        _passwordHandlerServiceMock
            .Received(1)
            .IsValidPassword(request.Password, "hashed-password");
        
        _jwtTokenHandlerMock
            .Received(1)
            .CreateToken(Arg.Any<SecurityTokenDescriptor>());
        _jwtTokenHandlerMock
            .Received(1)
            .WriteToken(Arg.Any<JwtSecurityToken>());
    }

    [Test]
    public async Task LoginAdminAsync_InvalidPassword()
    {
        // Arrange
        var request = new LoginAdminRequest
        {
            UserName = "admin",
            Password = "adminadmin"
        };
        var expectedResponse = new LoginAdminResponse();

        _adminRepositoryMock
            .GetAdminDetailsAsync(Arg.Any<LoginAdminRequest>())
            .Returns(new LoginAdminInformation
            {
                AdminId = 2,
                Name = "Peter",
                PasswordHash = "hashed-password"
            });
        
        _passwordHandlerServiceMock
            .IsValidPassword(Arg.Any<string>(), Arg.Any<string>())
            .Returns(false);
        
        // Act
        var result = await _target.LoginAdminAsync(request);
        
        // Assert
        result.ShouldBeEquivalentTo(expectedResponse);
        
        await _adminRepositoryMock
            .Received(1)
            .GetAdminDetailsAsync(Arg.Is<LoginAdminRequest>(r => AssertAreEquivalent(r, request)));
        
        _passwordHandlerServiceMock
            .Received(1)
            .IsValidPassword(request.Password, "hashed-password");
        
        _jwtTokenHandlerMock
            .DidNotReceive()
            .CreateToken(Arg.Any<SecurityTokenDescriptor>());
        _jwtTokenHandlerMock
            .DidNotReceive()
            .WriteToken(Arg.Any<JwtSecurityToken>());
    }

    [Test]
    public async Task GetAllOrdersAsync_Success()
    {
        // Arrange
        var firstOrderId = Guid.NewGuid();
        var secondOrderId = Guid.NewGuid();
        
        var expectedResponse = new GetAllOrdersResponse
        {
            GetAllOrderResponses = new List<GetAllOrderResponse>
            {
                new()
                {
                    OrderId = secondOrderId,
                    UserName = "Jane Doe",
                    OrderState = OrderState.WaitingToAccept,
                    OrderDate = DateTime.MaxValue,
                    OrderItems = new List<OrderItem>
                    {
                        new()
                        {
                            OrderId = 20,
                            PizzaName = "Picante",
                            Quantity = 1,
                            Price = 8.0m
                        }
                    }
                },
                new()
                {
                    OrderId = firstOrderId,
                    UserName = "John Doe",
                    OrderState = OrderState.Preparing,
                    OrderDate = DateTime.MinValue,
                    OrderItems = new List<OrderItem>
                    {
                        new()
                        {
                            OrderId = 10,
                            PizzaName = "Margherita",
                            Quantity = 2,
                            Price = 5.0m
                        }
                    }
                }
            }
        };
        
        _adminRepositoryMock
            .GetAllOrdersForTodayAsync()
            .Returns(new List<GetAllOrdersRawResponse>
            {
                new()
                {
                    OrderIdInteger = 10,
                    OrderIdGuid = firstOrderId,
                    UserName = "John Doe",
                    PizzaName = "Margherita",
                    Quantity = 2,
                    Price = 5.0m,
                    OrderState = OrderState.Preparing,
                    OrderDate = DateTime.MinValue
                },
                new()
                {
                    OrderIdInteger = 20,
                    OrderIdGuid = secondOrderId,
                    UserName = "Jane Doe",
                    PizzaName = "Picante",
                    Quantity = 1,
                    Price = 8.0m,
                    OrderState = OrderState.WaitingToAccept,
                    OrderDate = DateTime.MaxValue
                }
            });

        // Act
        var result = await _target.GetAllOrdersAsync();

        // Assert
        result.GetAllOrderResponses.ShouldBeEquivalentTo(expectedResponse.GetAllOrderResponses);
    }

    [Test]
    public async Task GetAllOrdersAsync_EmptyResponse()
    {
        // Arrange
        var expectedResponse = new GetAllOrdersResponse
        {
            GetAllOrderResponses = new List<GetAllOrderResponse>()
        };
        
        _adminRepositoryMock
            .GetAllOrdersForTodayAsync()
            .Returns(new List<GetAllOrdersRawResponse>());

        // Act
        var result = await _target.GetAllOrdersAsync();

        // Assert
        result.ShouldBeEquivalentTo(expectedResponse);
    }

    [Test]
    public async Task ChangeOrderStateAsync_Success()
    {
        // Arrange
        var request = new ChangeOrderStateRequest
        {
            OrderId = Guid.NewGuid(),
            NewOrderState = OrderState.Preparing
        };
        
        var expectedOrderStatusChangedNotification = new OrderStatusChangedNotification
        {
            OrderId = request.OrderId,
            NewOrderState = request.NewOrderState
        };

        _adminRepositoryMock
            .ChangeOrderStateAsync(Arg.Any<ChangeOrderStateRequest>())
            .Returns(1);

        // Act
        await _target.ChangeOrderStateAsync(request);

        // Assert
        await _adminRepositoryMock
            .Received(1)
            .ChangeOrderStateAsync(Arg.Is<ChangeOrderStateRequest>(r => AssertAreEquivalent(r, request)));

        await _hubContextMock
            .Received(1)
            .SendOrderStatusUpdateToUser(
                Arg.Is<string>(userId => userId == "1"),
                Arg.Is<OrderStatusChangedNotification>(notification => AssertAreEquivalent(notification, expectedOrderStatusChangedNotification)));
    }

    [Test]
    public async Task ChangeOrderStateAsync_UserNotFound()
    {
        // Arrange
        var request = new ChangeOrderStateRequest
        {
            OrderId = Guid.NewGuid(),
            NewOrderState = OrderState.Preparing
        };

        _adminRepositoryMock
            .ChangeOrderStateAsync(Arg.Any<ChangeOrderStateRequest>())
            .Returns(0);

        // Act
        await _target.ChangeOrderStateAsync(request);

        // Assert
        await _adminRepositoryMock
            .Received(1)
            .ChangeOrderStateAsync(Arg.Is<ChangeOrderStateRequest>(r => AssertAreEquivalent(r, request)));

        await _hubContextMock
            .DidNotReceive()
            .SendOrderStatusUpdateToUser(
                Arg.Any<string>(),
                Arg.Any<OrderStatusChangedNotification>());
    }
}