using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Cisco.Otel.Distribution.Tracing;
using Microsoft.Extensions.Configuration;

namespace Cisco.Otel.Distribution.UnitTests;

public class OptionsTests
{
    [Test]
    public void OptionsDefaultsTest()
    {
        var options = new CiscoOptions(new List<ExporterOptions> {new ExporterOptions.Console()});

        Assert.AreEqual(Constants.DefaultServiceName, options.ServiceName);
    }

    [Test]
    public void EmptyExportersListTest()
    {
        Assert.Throws<ArgumentException>(() =>
            {
                var options = new CiscoOptions(new List<ExporterOptions>());
            },
            "Must contain one or more exporter options");
    }

    [Test]
    public void ExporterOptionsMissingTokenTest()
    {
        Assert.Throws<ArgumentException>(() =>
            {
                var otlpGrpc = new ExporterOptions.OtlpGrpc(string.Empty);
            },
            "Cisco Token cannot be null");

        Assert.Throws<ArgumentException>(() =>
            {
                var otlpHttp = new ExporterOptions.OtlpHttp(string.Empty);
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

        Assert.AreEqual("test-application", options.ServiceName);
        Assert.IsTrue(options.ExporterOptions.Any());
        Assert.AreEqual(1, options.ExporterOptions.Count());
        Assert.IsTrue(options.ExporterOptions.First() is ExporterOptions.OtlpGrpc);
        
        var grpcExporter = (ExporterOptions.OtlpGrpc)options.ExporterOptions.First();
        
        Assert.AreEqual("my-cisco-token", grpcExporter.CiscoToken);
        Assert.AreEqual("http://localhost:4317", grpcExporter.CollectorEndpoint);
    }
    
    [Test]
    public void OptionsFromEnvironmentVariablesTest()
    {
        Environment.SetEnvironmentVariable(Constants.CiscoTokenEnvironmentVariableName, "my-cisco-token");
        Environment.SetEnvironmentVariable(Constants.ServiceNameEnvironmentVariableName, "test-application");
        Environment.SetEnvironmentVariable(Constants.ExporterTypeEnvironmentVariableName, "otlp-grpc");
        Environment.SetEnvironmentVariable(Constants.CollectorEndpointEnvironmentVariableName, "http://localhost:4317");
        
        var options = CiscoOptionsHelper.FromEnvironmentVariables();

        Assert.AreEqual("test-application", options.ServiceName);
        Assert.IsTrue(options.ExporterOptions.Any());
        Assert.AreEqual(1, options.ExporterOptions.Count());
        Assert.IsTrue(options.ExporterOptions.First() is ExporterOptions.OtlpGrpc);
        
        var grpcExporter = (ExporterOptions.OtlpGrpc)options.ExporterOptions.First();
        
        Assert.AreEqual("my-cisco-token", grpcExporter.CiscoToken);
        Assert.AreEqual("http://localhost:4317", grpcExporter.CollectorEndpoint);
    }
}