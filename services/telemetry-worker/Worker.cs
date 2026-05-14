using Npgsql;
using StackExchange.Redis;
using System.Text.Json;
using TelemetryContracts;

public class Worker : BackgroundService
{
	private readonly IDatabase _redis;
	private readonly string _connectionString;

	public Worker(
			IConnectionMultiplexer redis,
			IConfiguration configuration)
	{
		_redis = redis.GetDatabase();

		_connectionString =
			configuration.GetConnectionString("Postgres")!;
	}

	protected override async Task ExecuteAsync(
			CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var item = await _redis.ListRightPopAsync("telemetry_queue");

			if (item.IsNullOrEmpty)
			{
				await Task.Delay(200, stoppingToken);
				continue;
			}

			var telemetry =
				JsonSerializer.Deserialize<TelemetryEvent>(item!);

			if (telemetry is null)
				continue;

			Console.WriteLine(
					$"Processing {telemetry.DeviceId}");

			await using var conn =
				new NpgsqlConnection(_connectionString);

			await conn.OpenAsync(stoppingToken);

			const string sql = """
				INSERT INTO telemetry_raw
				(
				 device_id,
				 timestamp,
				 latitude,
				 longitude,
				 speed,
				 battery,
				 temperature
				)
				VALUES
				(
				 @device_id,
				 @timestamp,
				 @latitude,
				 @longitude,
				 @speed,
				 @battery,
				 @temperature
				)
				""";

			await using var cmd =
				new NpgsqlCommand(sql, conn);

			cmd.Parameters.AddWithValue(
					"device_id",
					telemetry.DeviceId);

			cmd.Parameters.AddWithValue(
					"timestamp",
					telemetry.Timestamp);

			cmd.Parameters.AddWithValue(
					"latitude",
					telemetry.Latitude);

			cmd.Parameters.AddWithValue(
					"longitude",
					telemetry.Longitude);

			cmd.Parameters.AddWithValue(
					"speed",
					telemetry.Speed);

			cmd.Parameters.AddWithValue(
					"battery",
					telemetry.Battery);

			cmd.Parameters.AddWithValue(
					"temperature",
					telemetry.Temperature);

			await cmd.ExecuteNonQueryAsync(stoppingToken);

			Console.WriteLine(
					$"Saved {telemetry.DeviceId} to Postgres");
		}
	}
}
