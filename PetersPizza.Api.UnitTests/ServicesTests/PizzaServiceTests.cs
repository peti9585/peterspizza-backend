using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Application.Interfaces.SignalR;
using PetersPizza.Api.Application.Services.Pizza;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Pizza;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ServicesTests;

[TestFixture]
public class PizzaServiceTests
{
    private IImageHandlerService _imageHandlerServiceMock;
    private IPizzaRepository _pizzaRepositoryMock;
    private IAdminService _adminServiceMock;
    private IAdminOrdersHub _hubContextMock;
    
    private PizzaService _target;

    [SetUp]
    public void Setup()
    {
        _imageHandlerServiceMock = Substitute.For<IImageHandlerService>();
        _pizzaRepositoryMock = Substitute.For<IPizzaRepository>();
        _adminServiceMock = Substitute.For<IAdminService>();
        _hubContextMock = Substitute.For<IAdminOrdersHub>();
        
        _target = new PizzaService(_pizzaRepositoryMock, _adminServiceMock, _imageHandlerServiceMock, _hubContextMock);
    }

    [Test]
    public async Task GetAllPizzasAsync_Success()
    {
        // Arrange
        var pizzaImageId = Guid.NewGuid();
        var expectedResponse = new GetAllPizzasResponse
        {
            GetAllPizzasResponses = new List<GetPizzaResponse>
            {
                new()
                {
                    PizzaId = 1,
                    PizzaName = "Margherita",
                    Description = "Italian pizza",
                    PizzaImageBytes = [1, 2, 3]
                }
            }
        };
        
        _pizzaRepositoryMock
            .GetAllPizzasAsync()
            .Returns(new GetAllPizzaDetailsResponse
            {
                GetAllPizzaDetailResponses =
                [
                    new GetAllPizzaDetailResponse
                    {
                        PizzaId = 1,
                        PizzaName = "Margherita",
                        Description = "Italian pizza",
                        PizzaImageId = pizzaImageId
                    }
                ]
            });
        _imageHandlerServiceMock
            .GetImageBytesByFileName(Arg.Any<string>())
            .Returns([1, 2, 3]);

        // Act
        var actualResponse = await _target.GetAllPizzasAsync();

        // Assert
        actualResponse.ShouldBeEquivalentTo(expectedResponse);
        
        await _pizzaRepositoryMock
            .Received(1)
            .GetAllPizzasAsync();
        _imageHandlerServiceMock
            .Received(1)
            .GetImageBytesByFileName(pizzaImageId.ToString());
    }

    [Test]
    public async Task InsertPizzaOrderAsync_Success()
    {
        // Arrange
        var request = new OrderPizzasRequest
        {
            UserId = 1,
            OrderId = Guid.NewGuid(),
            OrderPizzaRequests = new List<OrderPizzaRequest> { new() { PizzaId = 1, Quantity = 1 } }
        };
        var orders = new Models.Admin.GetAllOrdersResponse
        {
            GetAllOrderResponses = new List<Models.Admin.GetAllOrderResponse>
            {
                new()
                {
                    OrderId = request.OrderId,
                    UserName = "peti8595",
                    OrderDate = DateTime.MinValue,
                    OrderState = Models.Common.OrderState.ReadyToPickUp,
                    OrderItems = new List<Models.Admin.OrderItem>
                    {
                        new()
                        {
                            OrderId = 2,
                            PizzaName = "Margherita",
                            Quantity = 2,
                            Price = 6.5m
                        }
                    }
                }
            }
        };
        
        _pizzaRepositoryMock
            .InsertPizzaOrderAsync(request)
            .Returns(Task.CompletedTask);
        _adminServiceMock
            .GetAllOrdersAsync()
            .Returns(orders);
        
        // Act
        await _target.InsertPizzaOrderAsync(request);
        
        // Assert
        await _pizzaRepositoryMock
            .Received(1)
            .InsertPizzaOrderAsync(request);
        await _adminServiceMock
            .Received(1)
            .GetAllOrdersAsync();
        await _hubContextMock
            .Received(1)
            .SendNewOrderNotificationToAdmin(orders);

    }

    [Test]
    public async Task GetPizzasByIdsAsync_Success()
    {
        // Arrange
        var request = new [] { 1, 2, 3 };

        var expectedResponse = new GetPizzasByIdsResponse
        {
            GetPizzaResponses = new List<GetPizzaByIdResponse>
            {
                new() { PizzaId = 1, PizzaName = "Margherita", PizzaPrice = 5.0m }
            }
        };

        _pizzaRepositoryMock
            .GetPizzasByIdsAsync(request)
            .Returns(new GetPizzasByIdsResponse
            {
                GetPizzaResponses = new List<GetPizzaByIdResponse>
                {
                    new() { PizzaId = 1, PizzaName = "Margherita", PizzaPrice = 5.0m }
                }
            });

        // Act
        var actualResponse = await _target.GetPizzasByIdsAsync(request);

        // Assert
        actualResponse.ShouldBeEquivalentTo(expectedResponse);
        
        await _pizzaRepositoryMock
            .Received(1)
            .GetPizzasByIdsAsync(request);
    }
}