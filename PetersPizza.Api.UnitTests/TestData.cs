using System.IO;
using Microsoft.AspNetCore.Http;

namespace PetersPizza.Api.UnitTests;

internal static class TestData
{
    internal static FormFile GetFormFile() => new (new MemoryStream(), 0, 0, "test.jpg", "test/jpg");
}