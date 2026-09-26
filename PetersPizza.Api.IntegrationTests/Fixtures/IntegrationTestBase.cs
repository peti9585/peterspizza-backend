using System.Threading.Tasks;
using NUnit.Framework;

namespace PetersPizza.Api.IntegrationTests.Fixtures;

[TestFixture]
public abstract class IntegrationTestBase
{
    protected DatabaseFixture Fixture;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Fixture = new DatabaseFixture();
        
        await Fixture.InitializeAsync();
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await Fixture.DisposeAsync();
}