using StackExchange.Redis;
using TelemetryApi.Application;
using TelemetryApi.Contracts;
using TelemetryApi.Infrastructure;
using TelemetryContracts;
using TelemetryGrpc;
using Insights;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITelemetryService, TelemetryService>();
builder.Services.AddScoped<ITelemetryRepository, PostgresTelemetryRepository>();
builder.Services.AddScoped<ITelemetryQueue, RedisTelemetryQueue>();

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var config = ConfigurationOptions.Parse("redis:6379");
    config.AbortOnConnectFail = false;
    config.ConnectRetry = 10;
    config.ReconnectRetryPolicy = new ExponentialRetry(5000);

    return ConnectionMultiplexer.Connect(config);
});

builder.Services.AddGrpcClient<TelemetryProcessor.TelemetryProcessorClient>(o =>
{
    o.Address = new Uri("http://telemetry-grpc-worker:8081");
});

builder.Services.AddGrpcClient<DeviceInsights.DeviceInsightsClient>(o =>
{
    o.Address = new Uri("http://telemetry-insights-worker:50051");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/healthcheck", () =>
    Results.Ok(new { status = "healthy" }));

app.MapPost("/telemetry", async (
    TelemetryIngestRequest request,
    ITelemetryQueue queue,
    TelemetryProcessor.TelemetryProcessorClient grpcClient) =>
{
    var telemetryEvent = new TelemetryEvent
    {
        DeviceId = request.DeviceId,
        Timestamp = request.Timestamp,
        Latitude = request.Latitude,
        Longitude = request.Longitude,
        Speed = request.Speed,
        Battery = request.Battery,
        Temperature = request.Temperature
    };

    // fast path → gRPC ingestion worker
    if (request.Speed > 100)
    {
        await grpcClient.ProcessTelemetryAsync(new TelemetryRequest
        {
            DeviceId = request.DeviceId,
            Timestamp = request.Timestamp.ToString("O"),
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Speed = request.Speed,
            Battery = request.Battery,
            Temperature = request.Temperature
        });

        return Results.Ok(new { routed = "grpc" });
    }

    // slow path → redis
    await queue.EnqueueAsync(telemetryEvent);

	return Results.Accepted(value: new { routed = "queue" });
});

app.MapGet("/devices/insights", async (
    DeviceInsights.DeviceInsightsClient grpcClient) =>
{
    var response = await grpcClient.GetInsightsAsync(new InsightsRequest());
    return Results.Ok(response);
});

app.Run();
