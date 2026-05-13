using Grpc.Core;
using TelemetryGrpc;

namespace telemetry_grpc_worker.Services;

public class TelemetryProcessorService : TelemetryProcessor.TelemetryProcessorBase
{
    private readonly ILogger<TelemetryProcessorService> _logger;

    public TelemetryProcessorService(ILogger<TelemetryProcessorService> logger)
    {
        _logger = logger;
    }

    public override Task<TelemetryResponse> ProcessTelemetry(
        TelemetryRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "gRPC processed {DeviceId} speed={Speed}",
            request.DeviceId,
            request.Speed);

        return Task.FromResult(new TelemetryResponse
        {
            Success = true,
            Message = "Processed"
        });
    }
}
