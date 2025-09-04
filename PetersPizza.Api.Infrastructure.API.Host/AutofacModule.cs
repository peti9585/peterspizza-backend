using System.Diagnostics.CodeAnalysis;
using Autofac;
using FluentValidation;
using PetersPizza.Api.Infrastructure.API.Host.Validators;
using PetersPizza.Api.ViewModels.User;

namespace PetersPizza.Api.Infrastructure.API.Host;

[ExcludeFromCodeCoverage]
public sealed class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Validators
        builder.RegisterType<RegisterUserRequestValidator>().As<IValidator<RegisterUserRequest>>().SingleInstance();
        builder.RegisterType<LoginUserRequestValidator>().As<IValidator<LoginUserRequest>>().SingleInstance();
    }
}