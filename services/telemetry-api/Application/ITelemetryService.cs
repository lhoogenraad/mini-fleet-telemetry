using TelemetryContracts;

namespace TelemetryApi.Application;

public interface ITelemetryService
{
    Task IngestAsync(TelemetryEvent telemetry);
}
