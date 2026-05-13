using Npgsql;
using TelemetryApi.Domain;

namespace TelemetryApi.Infrastructure;

public class PostgresTelemetryRepository : ITelemetryRepository
{
    private readonly string _connectionString;

    public PostgresTelemetryRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Postgres")!;
    }

    public async Task InsertAsync(TelemetryEvent telemetry)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            INSERT INTO telemetry_raw
            (device_id, timestamp, latitude, longitude, speed, battery, temperature)
            VALUES
            (@device_id, @timestamp, @latitude, @longitude, @speed, @battery, @temperature)
        ", conn);

        cmd.Parameters.AddWithValue("device_id", telemetry.DeviceId);
        cmd.Parameters.AddWithValue("timestamp", telemetry.Timestamp);
        cmd.Parameters.AddWithValue("latitude", telemetry.Latitude);
        cmd.Parameters.AddWithValue("longitude", telemetry.Longitude);
        cmd.Parameters.AddWithValue("speed", telemetry.Speed);
        cmd.Parameters.AddWithValue("battery", telemetry.Battery);
        cmd.Parameters.AddWithValue("temperature", telemetry.Temperature);

        await cmd.ExecuteNonQueryAsync();
    }
}
