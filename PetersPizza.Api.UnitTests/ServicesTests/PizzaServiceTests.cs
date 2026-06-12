using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using PetersPizza.Api.Application.Interfaces.Services;
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
    
    private PizzaService _target;

    [SetUp]
    public void Setup()
    {
        _imageHandlerServiceMock = Substitute.For<IImageHandlerService>();
        _pizzaRepositoryMock = Substitute.For<IPizzaRepository>();
        
        _target = new PizzaService(_imageHandlerServiceMock, _pizzaRepositoryMock);
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
        
        _pizzaRepositoryMock.GetAllPizzasAsync()
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
        _imageHandlerServiceMock.GetImageBytesByFileName(Arg.Any<string>())
            .Returns([1, 2, 3]);

        // Act
        var actualResponse = await _target.GetAllPizzasAsync();

        // Assert
        actualResponse.ShouldBeEquivalentTo(expectedResponse);
        
        await _pizzaRepositoryMock.Received(1).GetAllPizzasAsync();
        _imageHandlerServiceMock.Received(1).GetImageBytesByFileName(pizzaImageId.ToString());
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
        _pizzaRepositoryMock.InsertPizzaOrderAsync(request)
            .Returns(Task.CompletedTask);
        
        // Act
        await _target.InsertPizzaOrderAsync(request);
        
        // Assert
        await _pizzaRepositoryMock.Received(1).InsertPizzaOrderAsync(request);
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

        _pizzaRepositoryMock.GetPizzasByIdsAsync(request)
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
        
        await _pizzaRepositoryMock.Received(1).GetPizzasByIdsAsync(request);
    }
}