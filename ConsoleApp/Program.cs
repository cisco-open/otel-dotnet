// See https://aka.ms/new-console-template for more information

using Cisco.Otel.Distribution.Tracing;

var ciscoOptions =
    new CiscoOptions(
        "my-cisco-token",
        "dotnet-console-example");

var tracerProvider = Tracer.Init(ciscoOptions);

Console.WriteLine("Hello, World!");