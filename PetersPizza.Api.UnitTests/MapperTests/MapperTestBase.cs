using NUnit.Framework;
using PetersPizza.Api.Infrastructure.Common.Mappers;

namespace PetersPizza.Api.UnitTests.MapperTests;

public class MapperTestBase
{
    protected Mapper Mapper;
    
    [SetUp]
    public void Setup() => Mapper = new Mapper();
}