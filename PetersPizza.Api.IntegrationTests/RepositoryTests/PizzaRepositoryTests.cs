using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PetersPizza.Api.Infrastructure.EntityFramework;
using PetersPizza.Api.Infrastructure.Repositories.Pizza;
using PetersPizza.Api.IntegrationTests.Fixtures;
using PetersPizza.Api.Models.Entities;
using PetersPizza.Api.Models.Pizza;
using Shouldly;

namespace PetersPizza.Api.IntegrationTests.RepositoryTests;

[TestFixture]
public class PizzaRepositoryTests : IntegrationTestBase
{
    private AppDbContext _dbContext;
    private PizzaRepository _target;
    
    [SetUp]
    public void SetUp()
    {
        _dbContext = Fixture.CreateDbContext();
        
        _target = new PizzaRepository(_dbContext);
    }

    [Test]
    public async Task GetAllPizzasAsync_ShouldReturnAllPizzas()
    {
        // Arrange
        var pizzas = new List<Pizza>
        {
            new()
            {
                Name = "Margherita",
                Description = "Classic pizza with tomato sauce and mozzarella cheese",
                Price = 6.50m,
                ImageId = Guid.NewGuid()
            },
            new()
            {
                Name = "Pepperoni",
                Description = "Spicy pepperoni with tomato sauce and mozzarella cheese",
                Price = 7.50m,
                ImageId = Guid.NewGuid()
            }
        };
            
        _dbContext.Pizza.AddRange(pizzas);
        await _dbContext.SaveChangesAsync();

        var expected = new GetAllPizzaDetailsResponse
        {
            GetAllPizzaDetailResponses = pizzas.Select(p => new GetAllPizzaDetailResponse
            {
                PizzaId = p.Id,
                PizzaName = p.Name,
                Description = p.Description,
                PizzaImageId = p.ImageId
            }).ToList()
        };

        // Act
        var result = await _target.GetAllPizzasAsync();

        // Assert
        expected.GetAllPizzaDetailResponses
            .ShouldAllBe(expectedPizza =>
                result.GetAllPizzaDetailResponses.Any(actualPizza =>
                    actualPizza.PizzaId == expectedPizza.PizzaId &&
                    actualPizza.PizzaName == expectedPizza.PizzaName &&
                    actualPizza.Description == expectedPizza.Description &&
                    actualPizza.PizzaImageId == expectedPizza.PizzaImageId));
    }

    [Test]
    public async Task GetPizzasByIdsAsync_ShouldReturnPizzasByIds()
    {
        // Arrange
        var pizza = new Pizza
        {
            Name = "Diavola",
            Description = "Extra spicy with tomato sauce, jalapeno and mozzarella cheese",
            Price = 8.70m,
            ImageId = Guid.NewGuid()
        };
        _dbContext.Pizza.Add(pizza);
        await _dbContext.SaveChangesAsync();

        var request = new List<int> { pizza.Id };

        var expected = new GetPizzasByIdsResponse
        {
            GetPizzaResponses = new List<GetPizzaByIdResponse>
            {
                new()
                {
                    PizzaId = pizza.Id,
                    PizzaName = pizza.Name,
                    PizzaPrice = pizza.Price
                }
            }
        };

        // Act
        var result = await _target.GetPizzasByIdsAsync(request);

        // Assert
        result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task GetPizzasByIdsAsync_ShouldReturnEmptyWhenNoPizzasFound()
    {
        // Arrange
        var request = new List<int> { 250 };

        var expected = new GetPizzasByIdsResponse{ GetPizzaResponses = new List<GetPizzaByIdResponse>() };

        // Act
        var result = await _target.GetPizzasByIdsAsync(request);

        // Assert
        result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task InsertPizzaOrderAsync_ShouldInsertOrder()
    {
        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            UserName = "johndoe",
            Email = "johndoe@gmail.com",
            PhoneNumber = "1234567890",
            Password = "password"
        };
        _dbContext.User.Add(user);
        
        var pizza = new Pizza
        {
            Name = "Hawaiian",
            Description = "Ham and pineapple with tomato sauce and mozzarella cheese",
            Price = 7.00m,
            ImageId = Guid.NewGuid()
        };
        _dbContext.Pizza.Add(pizza);
        
        await _dbContext.SaveChangesAsync();
        
        var orderId = Guid.NewGuid();
        var request = new OrderPizzasRequest
        {
            UserId = user.Id,
            OrderId = orderId,
            OrderPizzaRequests = new List<OrderPizzaRequest>
            {
                new()
                {
                    PizzaId = pizza.Id,
                    Quantity = 2
                }
            }
        };

        // Act
        await _target.InsertPizzaOrderAsync(request);

        // Assert
        var orders = _dbContext.Order
            .Where(x => x.UserId == user.Id)
            .AsNoTracking()
            .ToList();
        
        orders.Count.ShouldBe(1);
        
        var order = orders.First();
        order.OrderId.ShouldBe(orderId);
        order.PizzaId.ShouldBe(pizza.Id);
        order.UserId.ShouldBe(user.Id);
        order.Count.ShouldBe(2);
    }

    [Test]
    public async Task GetAllOrdersByIdAsync_ShouldReturnOrdersForUser()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Jane",
            LastName = "Doe",
            UserName = "janedoe",
            Email = "janedoe@gmail.com",
            PhoneNumber = "1234567891",
            Password = "password"
        };
        _dbContext.User.Add(user);

        var pizzas = new List<Pizza>
        {
            new()
            {
                Name = "Veggie",
                Description = "Loaded with vegetables and mozzarella cheese",
                Price = 6.80m,
                ImageId = Guid.NewGuid()
            },
            new()
            {
                Name = "BBQ Chicken",
                Description = "Grilled chicken with BBQ sauce and mozzarella cheese",
                Price = 8.20m,
                ImageId = Guid.NewGuid()
            }
        };
        _dbContext.Pizza.AddRange(pizzas);
        
        await _dbContext.SaveChangesAsync();
        
        var orderId = Guid.NewGuid();
        var request = new OrderPizzasRequest
        {
            UserId = user.Id,
            OrderId = orderId,
            OrderPizzaRequests = pizzas.Select(pizza => new OrderPizzaRequest
            {
                PizzaId = pizza.Id,
                Quantity = 1
            }).ToList()
        };
        
        await _target.InsertPizzaOrderAsync(request);

        // Act
        var result = await _target.GetAllOrdersByIdAsync(user.Id);

        // Assert
        var orderResponse = result.GetAllOrderResponses.First();
        orderResponse.OrderId.ShouldBe(orderId);
        orderResponse.OrderState.ShouldBe(Models.Common.OrderState.WaitingToAccept);
    }
}