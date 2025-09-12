using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Infrastructure.Interfaces.Mappers;
using PetersPizza.Api.ViewModels.User;

namespace PetersPizza.Api.Infrastructure.API.Host.Controllers;

public class UserController() : CarterModule("api/user")
{
    private const string Tag = "User";
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // User Registration
        app.MapPost<RegisterUserRequest>("/register", async (
            [FromBody]RegisterUserRequest request, 
            IValidator<RegisterUserRequest> validator,
            IMapper mapper,
            IUserService userService) =>
        {
            validator.ValidateAndThrow(request);
            var requestModel = mapper.Map(request);

            var userId = await userService.RegisterUserAsync(requestModel);

            return userId > 0 
                ? Results.Ok()
                : Results.Conflict();
        })
        .WithTags(Tag);
        
        // User Login
        app.MapPost<LoginUserRequest>("/login", async (
            [FromBody]LoginUserRequest request,
            IValidator<LoginUserRequest> validator,
            IMapper mapper,
            IUserService userService) =>
        {
            validator.ValidateAndThrow(request);
            var requestModel = mapper.Map(request);

            var response = await userService.LoginUserAsync(requestModel);
            
            if (string.IsNullOrEmpty(response.JwtToken) || string.IsNullOrWhiteSpace(response.Name))
            {
                return Results.NotFound();
            }
            
            var viewModel = mapper.Map(response);
            
            return Results.Ok(viewModel);
        })
        .WithTags(Tag);
        
        // Refresh JWT Token
        app.MapPost<RefreshJwtTokenRequest>("/refresh-token", async (
                [FromBody]RefreshJwtTokenRequest request,
                IValidator<RefreshJwtTokenRequest> validator,
                IUserService userService,
                IMapper mapper) => 
            {
                validator.ValidateAndThrow(request);
                
                var requestModel = mapper.Map(request);

                var response = await userService.RefreshJwtTokenAsync(requestModel);
                
                var viewModel = mapper.Map(response);
                
                return Results.Ok(viewModel);
            })
        .WithTags(Tag);
    }
}