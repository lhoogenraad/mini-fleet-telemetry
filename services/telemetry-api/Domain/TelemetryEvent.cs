namespace TelemetryApi.Domain;

public record TelemetryEvent
(
    string DeviceId,
    DateTime Timestamp,
    double Latitude,
    double Longitude,
    double Speed,
    double Battery,
    double Temperature
);
