# Otel-dotnet
This is an alpha version of the Cisco OpenTelemetry distribution for .Net.

## Getting Started

### Installing the Cisco.Otel.Distribution package

You can add the package to your application by using the following command:

`dotnet add package Cisco.Otel.Distribution --prerelease`

### Usage

To use the Cisco.Otel.Distribution distro with a .NET console application, add the following to the start of the application:
```c#
var options = 
    new CiscoOptions(
        new ExporterOptions[]
        {
            new ExporterOptions.OtlpGrpc("my-cisco-token", "http://localhost:4317"),
            new ExporterOptions.Console()
        });

var tracerProvider = Trace.Init(options);
```


To use the Cisco.Otel.Distribution distro with a .NET Core web application, add the following to the `Startup.cs` of the application:
```c#
var options = 
    new CiscoOptions(
        new ExporterOptions[]
        {
            new ExporterOptions.OtlpGrpc("my-cisco-token", "http://localhost:4317"),
            new ExporterOptions.Console()
        });
        
services.AddCiscoTracing(Configuration);
```

### Configuration

There are multiple ways of configuring the Cisco.Otel.Distribution distro. By manually creating an instance of `CiscoOptions.cs`, configuring `IConfiguration` instances (eg via appsettings.json), or adding environment variables:

|Environment variable|Appsettings key|Default|Description|
|-|-|-|-|
|`CISCO_TOKEN`|CiscoOptions.CiscoToken|-|`required` The Cisco account token|
|`OTEL_SERVICE_NAME`|CiscoOptions.ServiceName|`application`|`optional` The application name that will be set for traces|
|`OTEL_EXPORTER_TYPE`|CiscoOptions.ExporterOptions.Type|`otlp-grpc`|`optional` The exporter type to use. Multiple exporter option available via the Init method see example below|
|`OTEL_COLLECTOR_ENDPOINT`|CiscoOptions.ExporterOptions.Endpoint|`http://localhost:4317`|`optional` The address of the trace collector to send traces to|


Using appsettings.json:
```json
{
  "CiscoOptions": {
    "ServiceName": "my-application",
    "CiscoToken" : "my-cisco-token",
    "ExporterOptions": {
      "Type": "otlp-grpc",
      "Endpoint": "http://localhost:4317"
    }
  }
}
```

To create an instance of `CiscoOptions.cs` from environment variables:

```c#
var options = CiscoOptionsHelper.FromEnvironmentVariables();
```

To create an instance of `CiscoOptions.cs` from `IConfiguration` instances:

```c#
var configuration = 
    new ConfigurationBuilder()
        .AddJsonFile("appsettings.test.json")
        .Build();

var options = CiscoOptionsHelper.FromConfiguration(configuration);
```