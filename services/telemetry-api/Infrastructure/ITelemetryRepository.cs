using TelemetryApi.Domain;

namespace TelemetryApi.Infrastructure;

public interface ITelemetryRepository
{
	Task InsertAsync(TelemetryEvent telemetry);
}
