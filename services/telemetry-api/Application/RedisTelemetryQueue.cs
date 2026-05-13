using StackExchange.Redis;
using System.Text.Json;
using TelemetryContracts;
using TelemetryApi.Contracts;

namespace TelemetryApi.Application;

public class RedisTelemetryQueue : ITelemetryQueue
{
    private readonly IDatabase _db;

    public RedisTelemetryQueue(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task EnqueueAsync(TelemetryEvent evt)
    {
        var json = JsonSerializer.Serialize(evt);
        await _db.ListLeftPushAsync("telemetry_queue", json);
    }
}
