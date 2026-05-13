using TelemetryApi.Domain;

namespace TelemetryApi.Application;

public interface ITelemetryService
{
    Task IngestAsync(TelemetryEvent telemetry);
}
