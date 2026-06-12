using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Application.Services.Admin;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;
using Shouldly;

namespace PetersPizza.Api.UnitTests.ServicesTests;

[TestFixture]
public class AdminServiceTests
{
    private IImageHandlerService _imageHandlerServiceMock;
    private IAdminRepository _adminRepositoryMock;
    
    private AdminService _target;

    [SetUp]
    public void Setup()
    {
        _imageHandlerServiceMock = Substitute.For<IImageHandlerService>();
        _adminRepositoryMock = Substitute.For<IAdminRepository>();
        
        _target = new AdminService(_imageHandlerServiceMock, _adminRepositoryMock);
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