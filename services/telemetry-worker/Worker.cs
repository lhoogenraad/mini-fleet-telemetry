using StackExchange.Redis;
using System.Text.Json;
using TelemetryContracts;

public class Worker : BackgroundService
{
    private readonly IDatabase _db;

    public Worker(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var item = await _db.ListRightPopAsync("telemetry_queue");

            if (item.IsNullOrEmpty)
            {
                await Task.Delay(200);
                continue;
            }

            var telemetry = JsonSerializer.Deserialize<TelemetryEvent>(item!);

            Console.WriteLine($"Processing {telemetry.DeviceId}");

            // TODO: insert into Postgres here
        }
    }
}
