using System;
using System.Linq;
using NUnit.Framework;
using Cisco.Otel.Distribution.Tracing;
using Cisco.Opentelemetry.Specifications.Consts;
using Microsoft.Extensions.Configuration;

namespace Cisco.Otel.Distribution.UnitTests;

public class OptionsTests
{
    [Test]
    public void OptionsDefaultsTest()
    {
        var options = new CiscoOptions("Bearer my-cisco-token");

        Assert.AreEqual("Bearer my-cisco-token", options.CiscoToken);
        Assert.AreEqual(Constants.DefaultServiceName, options.ServiceName);
        Assert.IsTrue(options.ExporterOptions.Any());
        Assert.AreEqual(1, options.ExporterOptions.Count());
        Assert.AreEqual(Consts.DEFAULT_CISCO_DEBUG, options.Debug);
        Assert.AreEqual(Consts.DEFAULT_PAYLOADS_ENABLED, options.PayloadsEnabled);
        Assert.AreEqual(Consts.DEFAULT_MAX_PAYLOAD_SIZE, options.MaxPayloadSize);
        Assert.IsTrue(options.ExporterOptions.First() is ExporterOptions.OtlpHttp);
    }

    [Test]
    public void OptionsMissingTokenTest()
    {
        Assert.Throws<ArgumentException>(() =>
            {
                var emptyToken = new CiscoOptions("");
            },
            "Cisco Token cannot be null");

        Assert.Throws<ArgumentException>(() =>
            {
                var nullToken = new CiscoOptions(null!);
            },
            "Cisco Token cannot be null");
    }

    [Test]
    public void OptionsFromConfigurationFileTest()
    {
        var configuration =
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

        var options = CiscoOptionsHelper.FromConfiguration(configuration);

        Assert.AreEqual("Bearer my-cisco-token", options.CiscoToken);
        Assert.AreEqual("test-application", options.ServiceName);
        Assert.AreEqual(true, options.Debug);
        Assert.AreEqual(true, options.PayloadsEnabled);
        Assert.AreEqual(1023, options.MaxPayloadSize);
        Assert.IsTrue(options.ExporterOptions.Any());
        Assert.AreEqual(1, options.ExporterOptions.Count());
        Assert.IsTrue(options.ExporterOptions.First() is ExporterOptions.OtlpGrpc);

        var grpcExporter = (ExporterOptions.OtlpGrpc)options.ExporterOptions.First();

        Assert.AreEqual("http://localhost:4317", grpcExporter.CollectorEndpoint);
    }
}