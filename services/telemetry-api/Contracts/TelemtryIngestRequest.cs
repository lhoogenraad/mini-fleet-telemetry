namespace TelemetryApi.Contracts;

public class TelemetryIngestRequest
{
    public string DeviceId { get; set; } = default!;
    public DateTime Timestamp { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public double Speed { get; set; }
    public double Battery { get; set; }
    public double Temperature { get; set; }
}
