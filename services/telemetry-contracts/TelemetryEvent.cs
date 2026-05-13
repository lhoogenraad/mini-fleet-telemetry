namespace TelemetryContracts;

public class TelemetryEvent
{
    public string DeviceId { get; set; }
    public DateTime Timestamp { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Speed { get; set; }
    public double Battery { get; set; }
    public double Temperature { get; set; }
}
