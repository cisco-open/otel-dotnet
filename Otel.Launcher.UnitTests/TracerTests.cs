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
    public async Task HappyFlowTest()
    {
        try
        {
            Tracing.Trace.Init();
        
            await new HttpClient().GetStringAsync("https://example.com/");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            Assert.Fail(e.Message);
        }
    }
}