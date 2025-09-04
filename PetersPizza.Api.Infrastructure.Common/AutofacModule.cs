using System.Diagnostics.CodeAnalysis;
using Autofac;
using PetersPizza.Api.Infrastructure.Common.Mappers;

namespace PetersPizza.Api.Infrastructure.Common;

[ExcludeFromCodeCoverage]
public sealed class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Mapper>().AsImplementedInterfaces().SingleInstance();
    }
}