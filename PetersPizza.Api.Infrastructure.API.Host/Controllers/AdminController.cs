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
        app.MapPost<LoginAdminRequest>("/login", async (
                [FromBody] LoginAdminRequest request,
                IValidator<LoginAdminRequest> validator,
                IMapper mapper,
                IAdminService adminService) =>
            {
                validator.ValidateAndThrow(request);

                var requestModel = mapper.Map(request);

                var response = await adminService.LoginAdminAsync(requestModel);
                
                if (string.IsNullOrEmpty(response.JwtToken) || string.IsNullOrWhiteSpace(response.Name))
                {
                    return Results.NotFound();
                }

                var responseViewModel = mapper.Map(response);
                
                return Results.Ok(responseViewModel);
            })
            .WithTags(Tag);
            
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
            .DisableAntiforgery()
            .RequireAuthorization("Admin");
        
        app.MapPost<JwtTokenInformationRequest>("/permission", (
                [FromBody] JwtTokenInformationRequest request,
                IValidator<JwtTokenInformationRequest> validator,
                IMapper mapper,
                IAdminService adminService) =>
        {
            validator.ValidateAndThrow(request);
            
            var requestModel = mapper.Map(request);
            
            var response = adminService.ExtractJwtInformationFromToken(requestModel);
            
            var responseViewModel = mapper.Map(response);
            
            return Results.Ok(responseViewModel);
        })
        .WithTags(Tag)
        .RequireAuthorization("Admin");
        
        app.MapGet("orders/all", async (
                IAdminService adminService,
                IMapper mapper) =>
            {
                var response = await adminService.GetAllOrdersAsync();

                var responseViewModel = mapper.Map(response);
                
                return Results.Ok(responseViewModel);
            })
        .WithTags(Tag)
        .RequireAuthorization("Admin");
        
        app.MapPost("orders/change", async (
                [FromBody] ChangeOrderStateRequest request,
                IValidator<ChangeOrderStateRequest> validator,
                IMapper mapper,
                IAdminService adminService) =>
        {
            validator.ValidateAndThrow(request);

            var requestModel = mapper.Map(request);

            await adminService.ChangeOrderStateAsync(requestModel);

            return Results.NoContent();
        })
        .WithTags(Tag)
        .RequireAuthorization("Admin");
    }
}