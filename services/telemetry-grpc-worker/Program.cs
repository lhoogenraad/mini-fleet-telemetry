using telemetry_grpc_worker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<TelemetryProcessorService>();

app.MapGet("/", () =>
{
    return "gRPC worker running";
});

app.Run();
