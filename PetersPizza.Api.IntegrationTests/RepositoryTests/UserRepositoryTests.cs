using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PetersPizza.Api.Infrastructure.EntityFramework;
using PetersPizza.Api.Infrastructure.Repositories.User;
using PetersPizza.Api.IntegrationTests.Fixtures;
using PetersPizza.Api.Models.Entities;
using PetersPizza.Api.Models.User;
using Shouldly;

namespace PetersPizza.Api.IntegrationTests.RepositoryTests;

[TestFixture]
public class UserRepositoryTests : IntegrationTestBase
{
    private AppDbContext _dbContext;
    private UserRepository _target;
    
    [SetUp]
    public void SetUp()
    {
        _dbContext = Fixture.CreateDbContext();
        
        _target = new UserRepository(_dbContext);
    }

    [Test]
    public async Task RegisterUserAsync_ShouldAddUserToDatabase()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            UserName = "johndoe",
            Email = "johndoe@example.com",
            PhoneNumber = "1234567890",
            Password = "password"
        };

        // Act
        var result = await _target.RegisterUserAsync(request);

        // Assert
        result.ShouldNotBe(0); // Should not be default value, meaning a user was added
        
        var userInDb = await _dbContext.User.FindAsync(result);
        
        userInDb.ShouldNotBeNull();
        userInDb.FirstName.ShouldBe(request.FirstName);
        userInDb.LastName.ShouldBe(request.LastName);
        userInDb.UserName.ShouldBe(request.UserName);
        userInDb.Email.ShouldBe(request.Email);
        userInDb.PhoneNumber.ShouldBe(request.PhoneNumber);
        userInDb.Password.ShouldBe(request.Password);
    }

    [Test]
    public async Task GetUserDetailsAsync_ShouldReturnUserInformationWhenUserExists()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peterkis",
            Email = "peterkis@gmail.com",
            PhoneNumber = "1234567891",
            Password = "password"
        };
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();

        var request = new LoginUserRequest
        {
            UserName = "peterkis",
            Password = "password"
        };

        var expected = new LoginUserInformation
        {
            UserId = user.Id,
            Name = user.UserName,
            PasswordHash = user.Password
        };

        // Act
        var result = await _target.GetUserDetailsAsync(request);

        // Assert
        result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task GetUserDetailsAsync_ShouldReturnEmptyWhenUserDoesNotExist()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            UserName = "notexistinguser",
            Password = "password"
        };

        var expected = new LoginUserInformation();

        // Act
        var result = await _target.GetUserDetailsAsync(request);

        // Assert
        result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task UpsertRefreshTokenAsync_ShouldInsertNewTokenWhenNoExistingToken()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peterkis2",
            Email = "peterkis2@gmail.com",
            PhoneNumber = "1234567892",
            Password = "password"
        };
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var refreshTokenBefore = await _dbContext.UserRefreshToken
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        var refreshToken = Guid.NewGuid();
        
        // Act
        await _target.UpsertRefreshTokenAsync(user.Id, refreshToken);
        
        // Assert
        refreshTokenBefore.ShouldBeNull();
        
        var refreshTokenAfter = await _dbContext.UserRefreshToken
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        
        refreshTokenAfter.ShouldNotBeNull();
        refreshTokenAfter.UserId.ShouldBe(user.Id);
        refreshTokenAfter.RefreshToken.ShouldBe(refreshToken);
    }

    [Test]
    public async Task UpsertRefreshTokenAsync_ShouldUpdateExistingTokenWhenTokenExists()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peterkis3",
            Email = "peterkis3@gmail.com",
            PhoneNumber = "1234567893",
            Password = "password"
        };
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();

        var beforeRefreshToken = Guid.NewGuid();
        var userRefreshToken = new UserRefreshToken
        {
            UserId = user.Id,
            RefreshToken = beforeRefreshToken,
            ExpirationDate = DateTime.UtcNow
        };
        _dbContext.UserRefreshToken.Add(userRefreshToken);
        await _dbContext.SaveChangesAsync();
        
        var newRefreshToken = Guid.NewGuid();
        
        // Act
        await _target.UpsertRefreshTokenAsync(user.Id, newRefreshToken);
        
        // Assert
        var refreshTokenAfter = await _dbContext.UserRefreshToken
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        
        refreshTokenAfter.ShouldNotBeNull();
        refreshTokenAfter.UserId.ShouldBe(user.Id);
        refreshTokenAfter.RefreshToken.ShouldBe(newRefreshToken);
        refreshTokenAfter.ExpirationDate.Day.ShouldBe(DateTime.UtcNow.AddDays(1).Day);
    }

    [Test]
    public async Task GetUserByRefreshTokenAsync_ShouldReturnUserInfoWhenTokenExists()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peterkis4",
            Email = "peterkis4@gmail.com",
            PhoneNumber = "1234567894",
            Password = "password"
        };
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();

        var refreshToken = Guid.NewGuid();
        var userRefreshToken = new UserRefreshToken
        {
            UserId = user.Id,
            RefreshToken = refreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(1)
        };
        _dbContext.UserRefreshToken.Add(userRefreshToken);
        await _dbContext.SaveChangesAsync();

        var expected = new UserInfo
        {
            Id = user.Id,
            UserName = user.UserName
        };
        
        // Act
        var result = await _target.GetUserByRefreshTokenAsync(refreshToken);
        
        // Assert
        result.ShouldBeEquivalentTo(expected);
    }
    
    [Test]
    public async Task GetUserByRefreshTokenAsync_ShouldNotReturnUserInfoWhenTokenExpired()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peterkis5",
            Email = "peterkis5@gmail.com",
            PhoneNumber = "1234567895",
            Password = "password"
        };
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();

        var refreshToken = Guid.NewGuid();
        var userRefreshToken = new UserRefreshToken
        {
            UserId = user.Id,
            RefreshToken = refreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(-1)
        };
        _dbContext.UserRefreshToken.Add(userRefreshToken);
        await _dbContext.SaveChangesAsync();

        var expected = new UserInfo();
        
        // Act
        var result = await _target.GetUserByRefreshTokenAsync(refreshToken);
        
        // Assert
        result.ShouldBeEquivalentTo(expected);
    }
    
    [Test]
    public async Task GetUserByIdAsync_ShouldReturnUserInfoWhenUserExists()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peterkis6",
            Email = "peterkis6@gmail.com",
            PhoneNumber = "1234567896",
            Password = "password"
        };
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();

        var expected = new GetUserDetailsByIdResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        
        // Act
        var result = await _target.GetUserByIdAsync(user.Id);
        
        // Assert
        result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldNotReturnUserInfoWhenUserDoesNotExist()
    {
        // Arrange
        var expected = new GetUserDetailsByIdResponse();
        
        // Act
        var result = await _target.GetUserByIdAsync(250); // Assuming this user ID does not exist in the test database
        
        // Assert
        result.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public async Task AreUserValuesUniqueAsync_ShouldReturnTrueWhenValuesAreUnique()
    {
        // Act
        var result = await _target.AreUserValuesUniqueAsync("0957432768", "uniqueemail@gmail.com", 17936);
        
        // Assert
        result.ShouldBeTrue();
    }

    [Test]
    public async Task AreUserValuesUniqueAsync_ShouldReturnFalseWhenValuesAreNotUnique()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peterkis7",
            Email = "peterkis7@gmail.com",
            PhoneNumber = "1234567897",
            Password = "password"
        };
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var result = await _target.AreUserValuesUniqueAsync("1234567897", "peterkis7@gmail.com", 200);
        
        // Assert
        result.ShouldBeFalse();
    }
    
    [Test]
    public async Task UpdateUserAsync_ShouldUpdateUserInformation()
    {
        // Arrange
        var user = new User
        {
            FirstName = "Peter",
            LastName = "Kis",
            UserName = "peterkis8",
            Email = "peterkis8@gmail.com",
            PhoneNumber = "1234567898",
            Password = "password"
        };
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();

        var updateRequest = new UpdateUserRequest
        {
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            Email = "updatedemail@gmail.com",
            PhoneNumber = "0987654321"
        };
        
        // Act
        await _target.UpdateUserAsync(updateRequest, user.Id);
        
        // Assert
        var updatedUser = await _dbContext.User
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == user.Id);
        
        updatedUser.ShouldNotBeNull();
        updatedUser.FirstName.ShouldBe(updateRequest.FirstName);
        updatedUser.LastName.ShouldBe(updateRequest.LastName);
        updatedUser.Email.ShouldBe(updateRequest.Email);
        updatedUser.PhoneNumber.ShouldBe(updateRequest.PhoneNumber);
    }
}