using TelemetryContracts;
using TelemetryApi.Infrastructure;

namespace TelemetryApi.Application;

public class TelemetryService : ITelemetryService
{
    private readonly ITelemetryRepository _repo;

    public TelemetryService(ITelemetryRepository repo)
    {
        _repo = repo;
    }

    public Task IngestAsync(TelemetryEvent telemetry)
    {
        return _repo.InsertAsync(telemetry);
    }
}
