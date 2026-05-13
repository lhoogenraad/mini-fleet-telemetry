using TelemetryApi.Application;
using TelemetryApi.Contracts;
using TelemetryContracts;
using TelemetryApi.Infrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITelemetryService, TelemetryService>();
builder.Services.AddScoped<ITelemetryRepository, PostgresTelemetryRepository>();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
{
    var config = ConfigurationOptions.Parse("redis:6379");
    config.AbortOnConnectFail = false;
    config.ConnectRetry = 10;
    config.ReconnectRetryPolicy = new ExponentialRetry(5000);

    return ConnectionMultiplexer.Connect(config);
});

builder.Services.AddScoped<ITelemetryQueue, RedisTelemetryQueue>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/healthcheck", () =>
{
    return "?Hello world?";
})
.WithName("Healthcheck")
.WithOpenApi();

app.MapPost("/telemetry", async (
    TelemetryEvent request,
    ITelemetryQueue queue) =>
{
    await queue.EnqueueAsync(request);
    return Results.Accepted();
});

app.Run();

