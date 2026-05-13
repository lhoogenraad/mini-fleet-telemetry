using TelemetryApi.Application;
using TelemetryApi.Contracts;
using TelemetryApi.Domain;
using TelemetryApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITelemetryService, TelemetryService>();
builder.Services.AddScoped<ITelemetryRepository, PostgresTelemetryRepository>();

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
    TelemetryIngestRequest request,
    ITelemetryService service) =>
{
    var domainEvent = new TelemetryEvent(
        request.DeviceId,
        request.Timestamp,
        request.Latitude,
        request.Longitude,
        request.Speed,
        request.Battery,
        request.Temperature
    );

    await service.IngestAsync(domainEvent);

    return Results.Accepted();
});

app.Run();

