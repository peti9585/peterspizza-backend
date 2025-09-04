using System.Diagnostics.CodeAnalysis;
using Autofac;

namespace PetersPizza.Api.Service;

[ExcludeFromCodeCoverage]
internal static class AutofacConfiguration
{
    public static void CreateContainer(ContainerBuilder containerBuilder)
    {
        // Modules
        containerBuilder.RegisterModule<PetersPizza.Api.Infrastructure.API.Host.AutofacModule>();
        containerBuilder.RegisterModule<Application.AutofacModule>();
        containerBuilder.RegisterModule<Infrastructure.Common.AutofacModule>();
        containerBuilder.RegisterModule<Infrastructure.Repositories.AutofacModule>();
    }
}