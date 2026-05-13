using TelemetryContracts;
namespace TelemetryApi.Application;

public interface ITelemetryQueue
{
    Task EnqueueAsync(TelemetryEvent evt);
}
