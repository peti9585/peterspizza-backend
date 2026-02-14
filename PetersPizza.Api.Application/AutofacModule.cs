using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using Autofac;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PetersPizza.Api.Application.BusinessValidators;
using PetersPizza.Api.Application.Services.Admin;
using PetersPizza.Api.Application.Services.Pizza;
using PetersPizza.Api.Application.Services.User;
using PetersPizza.Api.Models.Admin;
using PetersPizza.Api.Models.User;

namespace PetersPizza.Api.Application;

[ExcludeFromCodeCoverage]
public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Services
        builder.RegisterType<UserService>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<AdminService>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<PizzaService>().AsImplementedInterfaces().SingleInstance();
        
        // Identity
        builder.RegisterType<PasswordHasher<RegisterUserRequest>>().AsSelf().SingleInstance();
        builder.RegisterType<PasswordHasher<LoginAdminRequest>>().AsSelf().SingleInstance();
        builder.RegisterType<JwtSecurityTokenHandler>().AsSelf().InstancePerDependency();
        
        // Business validators
        builder.RegisterType<UserAlreadyExistsValidator>().As<IValidator<(UpdateUserRequest, int)>>().SingleInstance();
    }
}