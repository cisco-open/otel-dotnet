using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Otel.Launcher.UnitTests;

public class TracerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void HappyPathTest()
    {
        try
        {
            Tracing.Trace.Init();

            new HttpClient().GetStringAsync("https://example.com").Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            Assert.Fail(e.Message);
        }
    }
}