using System.Diagnostics.CodeAnalysis;
using Autofac;
using PetersPizza.Api.Infrastructure.Repositories.Admin;
using PetersPizza.Api.Infrastructure.Repositories.User;

namespace PetersPizza.Api.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserRepository>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<AdminRepository>().AsImplementedInterfaces().SingleInstance();
    }
}