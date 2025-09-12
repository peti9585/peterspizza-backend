using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using Autofac;
using Microsoft.AspNetCore.Identity;
using PetersPizza.Api.Application.Services.Admin;
using PetersPizza.Api.Application.Services.Pizza;
using PetersPizza.Api.Application.Services.User;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Application;

[ExcludeFromCodeCoverage]
public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserService>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<AdminService>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<PizzaService>().AsImplementedInterfaces().SingleInstance();
        
        builder.RegisterType<PasswordHasher<RegisterUserRequest>>().AsSelf().SingleInstance();
        builder.RegisterType<JwtSecurityTokenHandler>().AsSelf().SingleInstance();
    }
}