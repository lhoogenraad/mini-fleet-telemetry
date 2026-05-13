using TelemetryApi.Domain;

namespace TelemetryApi.Application;

public class TelemetryService : ITelemetryService
{
    public Task IngestAsync(TelemetryEvent telemetry)
    {
        // for now: just log / placeholder
        Console.WriteLine($"Received telemetry for {telemetry.DeviceId}");

        return Task.CompletedTask;
    }
}
