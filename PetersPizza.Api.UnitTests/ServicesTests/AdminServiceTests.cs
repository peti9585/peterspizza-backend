using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Application.Services.Admin;
using PetersPizza.Api.Application.SignalR;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ServicesTests;

[TestFixture]
public class AdminServiceTests
{
    private IImageHandlerService _imageHandlerServiceMock;
    private IAdminRepository _adminRepositoryMock;
    private IConfiguration _configurationMock;
    private IPasswordHandlerService<LoginAdminRequest> _passwordHandlerServiceMock;
    private JwtSecurityTokenHandler _jwtTokenHandlerMock;
    private IHubContext<UserOrdersHub> _hubContextMock;
    
    private AdminService _target;

    [SetUp]
    public void Setup()
    {
        _imageHandlerServiceMock = Substitute.For<IImageHandlerService>();
        _adminRepositoryMock = Substitute.For<IAdminRepository>();
        _configurationMock = Substitute.For<IConfiguration>();
        _passwordHandlerServiceMock = Substitute.For<IPasswordHandlerService<LoginAdminRequest>>();
        _jwtTokenHandlerMock = Substitute.For<JwtSecurityTokenHandler>();
        _hubContextMock = Substitute.For<IHubContext<UserOrdersHub>>();
        
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
    public void UploadPizzaAsync_WhenInsertPizzaAsync_ThrowsException()
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
        var actualException = Assert.ThrowsAsync<InvalidOperationException>(() => _target.UploadPizzaAsync(request));

        // Assert
        actualException.ShouldBe(expectedException);
    }
}