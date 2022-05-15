# otel-dotnet

This package provides OpenTelemetry-compliant tracing to .NET applications for the collection of distributed tracing and performance metrics in [Cisco Telescope](https://console.telescope.app/?utm_source=github).

## Contents

- [otel-dotnet](#otel-dotnet)
  - [Contents](#contents)
  - [Installation](#installation)
    - [Install packages](#install-packages)
    - [Library initialization](#library-initialization)
      - [.NET Core](#net-core)
      - [.NET](#net)
    - [OpenTelemetry Collector Configuration](#opentelemetry-collector-configuration)
      - [Configure custom trace exporter](#configure-custom-trace-exporter)
      - [Configure custom OpenTelemetry collector to export trace data to Cisco Telescope's external collector.](#configure-custom-opentelemetry-collector-to-export-trace-data-to-cisco-telescopes-external-collector)
    - [Existing OpenTelemetry Instrumentation](#existing-opentelemetry-instrumentation)
  - [Supported Runtimes](#supported-runtimes)
  - [Supported Libraries](#supported-libraries)
  - [Configuration](#configuration)
  - [Getting Help](#getting-help)
  - [Opening Issues](#opening-issues)
  - [License](#license)

## Installation

### Install packages

To install Cisco OpenTelemetry Distribution simply run:

```sh
dotnet add package Cisco.Telescope
```

### Library initialization

> Cisco OpenTelemetry Distribution is activated and instruments the supported libraries once the module is imported.

#### .NET Core

```c#
using Cisco.Otel.Distribution.Tracing;

var options = 
    new CiscoOptions(
        "cisco-token",
        "my-app-name");

services.AddCiscoTracing(options);
```

#### .NET

```c#
using Cisco.Otel.Distribution.Tracing;

var options = 
    new CiscoOptions(
        "cisco-token",
        "my-app-name");

Tracer.Init(options);
```

### OpenTelemetry Collector Configuration

> By default, Cisco OpenTelemetry Distribution exports data directly to [Cisco Telescope's](https://console.telescope.app/?utm_source=github) infrastructure backend.
> **Existing** OpenTelemetery Collector is supported, the following configuration can be applied

#### Configure custom trace exporter

> Cisco OpenTelemetry Distribution supports configure multiple custom exporters.
> Example for create OtlpGrpc Span exporter to local OpenTelemetry collector:

```c#
using Cisco.Otel.Distribution.Tracing;

var options =
    new CiscoOptions(
        "telescope-token",
        "my-app-name",
        new ExporterOptions[]
        {
            new ExporterOptions.OtlpGrpc("http://localhost:4317"),
        });

services.AddCiscoTracing(options);
```

#### Configure custom OpenTelemetry collector to export trace data to [Cisco Telescope's](https://console.telescope.app/?utm_source=github) external collector.

```yaml
collector.yaml ...

exporters:
  otlphttp:
    traces_endpoint: https://production.cisco-udp.com/trace-collector
    headers:
      authorization: <Your Telescope Token>
    compression: gzip


service:
  pipelines:
    traces:
      exporters: [otlphttp]
```

### Existing OpenTelemetry Instrumentation

> Notice: Only relevant if interested in streaming existing OpenTelemetry workloads.
> [Cisco Telescope](https://console.telescope.app/?utm_source=github). supports native OpenTelemetery traces.

```c#
var tracerProvider = 
    Sdk.CreateTracerProviderBuilder()
        .SetResourceBuilder(ResourceBuilder.CreateDefault())
        .AddOtlpExporter(options =>
        {
            options.Protocol = OtlpExportProtocol.HttpProtobuf;
            options.Endpoint = new Uri("https://production.cisco-udp.com/trace-collector");
            options.Headers = $"authorization=Your Telescope Token";
        })
        .Build();
```

## Supported Runtimes

| Platform Version                  | Supported |
|-----------------------------------| --------- |
| .NET Framework `4.6.2 and higher` | ✅        |
| .NET Core `3.1`                   | ✅        |
| .NET `5.0`                        | ✅        |
| .NET `6.0`                        | ✅        |

## Supported Libraries

Cisco OpenTelemetry .NET Distribution provides out-of-the-box instrumentation (tracing) and advanced **payload collections** for many popular frameworks and libraries.


| Library         | Supported |
|-----------------| --------- |
| HTTP Client     | ✅        |
| Grpc.Net Client | ✅        |
| SQL Client      | ✅        |
| ASP.NET Core    | ✅        |
| ASP.NET         | ✅        |

## Configuration

Advanced options can be configured as parameters to the Init() method:

| AppSettings key | Env                    | Type    | Default       | Description                                                       |
|-----------------| ---------------------- | ------- | ------------- | ----------------------------------------------------------------- |
| CiscoOptions.CiscoToken      | CISCO_TOKEN            | string  | -             | Cisco account token                                               |
| CiscoOptions.ServiceName     | OTEL_SERVICE_NAME      | string  | `application` | Application name that will be set for traces                      |

Exporter options:

| AppSettings key         | Env                     | Type                | Default                                               | Description                                                                                                                     |
| ----------------- | ----------------------- | ------------------- | ----------------------------------------------------- |---------------------------------------------------------------------------------------------------------------------------------|
| CiscoOptions.ExporterOptions.Endpoint | OTEL_COLLECTOR_ENDPOINT | string              | `http://localhost:4317` | The address of the trace collector to send traces to                                                                            |
| CiscoOptions.ExporterOptions.Type              | OTEL_EXPORTER_TYPE      | string              | `otlp-grpc`                                           | The exporter type to use. Multiple exporter options available via IConfiguration instances and Init function. See example below |


To configure options from IConfiguration instances (eg via appsettings.json):
```c#
var configuration = 
    new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

var options = CiscoOptionsHelper.FromConfiguration(configuration);
```

## Getting Help

If you have any issue around using the library or the product, please don't hesitate to:

- Use the [documentation](https://docs.telescope.app).
- Use the help widget inside the product.
- Open an issue in GitHub.

## Opening Issues

If you encounter a bug with the Cisco OpenTelemetry Distribution for .NET, we want to hear about it.

When opening a new issue, please provide as much information about the environment:

- Library version, .NET version, dependencies, etc.
- Snippet of the usage.
- A reproducible example can really help.

The GitHub issues are intended for bug reports and feature requests.
For help and questions about [Cisco Telescope](https://console.telescope.app/?utm_source=github), use the help widget inside the product.

## License

Provided under the Apache 2.0. See LICENSE for details.

Copyright 2022, Cisco
