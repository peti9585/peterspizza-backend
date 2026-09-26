using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PetersPizza.Api.Infrastructure.EntityFramework;
using PetersPizza.Api.Infrastructure.Repositories.Admin;
using PetersPizza.Api.IntegrationTests.Fixtures;
using PetersPizza.Api.Models.Admin;
using PetersPizza.Api.Models.Entities;
using Shouldly;

namespace PetersPizza.Api.IntegrationTests.RepositoryTests;

[TestFixture]
public class AdminRepositoryTests : IntegrationTestBase
{
    private AppDbContext _dbContext;
    private AdminRepository _target;
    
    [SetUp]
    public void SetUp()
    {
        _dbContext = Fixture.CreateDbContext();
        
        _target = new AdminRepository(_dbContext);
    }
    
    [Test]
    public async Task InsertPizzaAsync_ShouldInsertPizzaIntoDatabase()
    {
        // Arrange
        var request = new InsertPizzaRequest
        {
            PizzaName = "InsertPizza Test",
            Description = "InsertPizza Description",
            PizzaPrice = 9.50m,
            PizzaImageId = Guid.NewGuid()
        };

        // Act
        await _target.InsertPizzaAsync(request);

        // Assert
        var insertedPizza = await _dbContext.Pizza.FirstOrDefaultAsync(p => p.Name == request.PizzaName);

        insertedPizza.ShouldNotBeNull();
        insertedPizza.Name.ShouldBe(request.PizzaName);
        insertedPizza.Description.ShouldBe(request.Description);
        insertedPizza.Price.ShouldBe(request.PizzaPrice);
        insertedPizza.ImageId.ShouldBe(request.PizzaImageId);
    }

    [Test]
    public async Task GetAdminDetailsAsync_ShouldReturnAdminDetails()
    {
        // Arrange
        var admin = new Admin
        {
            Name = "admin",
            Password = "password"
        };
        _dbContext.Add(admin);
        await _dbContext.SaveChangesAsync();

        var request = new LoginAdminRequest
        {
            UserName = "admin",
            Password = "password"
        };
        
        var expected = new LoginAdminInformation
        {
            AdminId = admin.Id,
            Name = admin.Name,
            PasswordHash = admin.Password
        };

        // Act
        var result = await _target.GetAdminDetailsAsync(request);

        // Assert
        result.ShouldBeEquivalentTo(expected);
    }
    
    [Test]
    public async Task GetAdminDetailsAsync_ShouldReturnEmptyWhenAdminNotFound()
    {
        // Arrange
        var request = new LoginAdminRequest
        {
            UserName = "notfound",
            Password = "password"
        };

        // Act
        var result = await _target.GetAdminDetailsAsync(request);

        // Assert
        result.ShouldBeEquivalentTo(new LoginAdminInformation());
    }

    [Test]
    public async Task GetAllOrdersForTodayAsync_ShouldReturnOrdersForToday()
    {
        // Arrange
        var pizza = new Pizza
        {
            Name = "GetAllOrdersForToday Pizza",
            Description = "GetAllOrdersForToday Description",
            Price = 9.50m,
            ImageId = Guid.NewGuid()
        };
        _dbContext.Add(pizza);
        await _dbContext.SaveChangesAsync();

        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            UserName = "johndoe",
            Email = "johndoe@gmail.com",
            PhoneNumber = "1234567890",
            Password = "password123"
        };
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            OrderId = orderId,
            UserId = user.Id,
            PizzaId = pizza.Id,
            Count = 2,
            OrderStateId = (int)Models.Common.OrderState.WaitingToAccept,
            OrderDate = DateTime.UtcNow
        };
        _dbContext.Add(order);
        await _dbContext.SaveChangesAsync();

        var expected = new GetAllOrdersRawResponse
        {
            OrderIdInteger = order.Id,
            OrderIdGuid = order.OrderId,
            UserName = user.FirstName + " " + user.LastName,
            PizzaName = pizza.Name,
            Price = pizza.Price,
            Quantity = order.Count,
            OrderState = (Models.Common.OrderState)order.OrderStateId,
            OrderDate = order.OrderDate
        };

        // Act
        var result = await _target.GetAllOrdersForTodayAsync();

        // Assert
        var firstResult = result.FirstOrDefault();
        
        firstResult.ShouldNotBeNull();
        firstResult.OrderIdInteger.ShouldBe(expected.OrderIdInteger);
        firstResult.OrderIdGuid.ShouldBe(expected.OrderIdGuid);
        firstResult.UserName.ShouldBe(expected.UserName);
        firstResult.PizzaName.ShouldBe(expected.PizzaName);
        firstResult.Price.ShouldBe(expected.Price);
        firstResult.Quantity.ShouldBe(expected.Quantity);
        firstResult.OrderState.ShouldBe(expected.OrderState);
        firstResult.OrderDate.Day.ShouldBe(expected.OrderDate.Day);
    }

    [Test]
    public async Task GetAllOrdersForTodayAsync_ShouldNotReturnOrdersForOtherDays()
    {
        // Arrange
        var pizza = new Pizza
        {
            Name = "GetAllOrdersForToday Pizza 2",
            Description = "GetAllOrdersForToday Description 2",
            Price = 9.50m,
            ImageId = Guid.NewGuid()
        };
        _dbContext.Add(pizza);
        await _dbContext.SaveChangesAsync();

        var user = new User
        {
            FirstName = "Jane",
            LastName = "Doe",
            UserName = "janedoe",
            Email = "janedoe@gmail.com",
            PhoneNumber = "1234567891",
            Password = "password123"
        };
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            OrderId = orderId,
            UserId = user.Id,
            PizzaId = pizza.Id,
            Count = 2,
            OrderStateId = (int)Models.Common.OrderState.WaitingToAccept,
            OrderDate = DateTime.UtcNow.AddDays(-1) // Set the date to the past
        };
        _dbContext.Add(order);
        await _dbContext.SaveChangesAsync();

        var expected = new List<GetAllOrdersRawResponse>();

        // Act
        var result = await _target.GetAllOrdersForTodayAsync();

        // Assert
        result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task ChangeOrderStateAsync_ShouldChangeOrderState()
    {
        // Arrange
        var pizza = new Pizza
        {
            Name = "GetAllOrdersForToday Pizza 3",
            Description = "GetAllOrdersForToday Description 3",
            Price = 9.50m,
            ImageId = Guid.NewGuid()
        };
        _dbContext.Add(pizza);
        await _dbContext.SaveChangesAsync();

        var user = new User
        {
            FirstName = "Jill",
            LastName = "Doe",
            UserName = "jilldoe",
            Email = "jilldoe@gmail.com",
            PhoneNumber = "1234567892",
            Password = "password123"
        };
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            OrderId = orderId,
            UserId = user.Id,
            PizzaId = pizza.Id,
            Count = 2,
            OrderStateId = (int)Models.Common.OrderState.WaitingToAccept,
            OrderDate = DateTime.UtcNow.AddDays(3) // Set the date to the future
        };
        _dbContext.Add(order);
        await _dbContext.SaveChangesAsync();

        var request = new ChangeOrderStateRequest
        {
            OrderId = orderId,
            NewOrderState = Models.Common.OrderState.Preparing
        };

        // Act
        var result = await _target.ChangeOrderStateAsync(request);

        // Assert
        result.ShouldBeEquivalentTo(user.Id);
        
        _dbContext.Order
            .Where(o => o.OrderId == orderId)
            .AsNoTracking()
            .Select(o => o.OrderStateId)
            .Single()
            .ShouldBe((int)Models.Common.OrderState.Preparing);
    }
}