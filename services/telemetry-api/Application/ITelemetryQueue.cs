using TelemetryApi.Domain;
namespace TelemetryApi.Application;

public interface ITelemetryQueue
{
    Task EnqueueAsync(TelemetryEvent evt);
}
