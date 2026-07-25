using Shouldly;

namespace PetersPizza.Api.UnitTests.ServicesTests;

public abstract class ServiceTestBase
{
    protected static bool AssertAreEquivalent<T1, T2>(T1 actual, T2 expected) where T1 : class where T2 : class
    {
        actual.ShouldBeEquivalentTo(expected);

        return true;
    }
}