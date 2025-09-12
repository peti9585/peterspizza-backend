using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.ViewModels.Admin;

namespace PetersPizza.Api.Infrastructure.API.Host.Controllers;

public class AdminController() : CarterModule("api/admin")
{
    private const string Tag = "Admin";
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // Upload Pizza
        app.MapPost<UploadPizzaRequest>("/upload-pizza", async (
                [FromForm] UploadPizzaRequest request,
                IValidator<UploadPizzaRequest> validator,
                IMapper mapper,
                IAdminService adminService) =>
            {
                validator.ValidateAndThrow(request);

                var requestModel = mapper.Map(request);

                await adminService.UploadPizzaAsync(requestModel);

                return Results.Ok();
            })
            .WithTags(Tag)
            .DisableAntiforgery();
        //.RequireAuthorization("Admin"); TODO: Set up table for roles
    }
}