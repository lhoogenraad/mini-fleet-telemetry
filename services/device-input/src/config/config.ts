export const config = {
  apiUrl: process.env.TELEMETRY_API_URL ?? "http://localhost:8080",
  deviceCount: Number(process.env.DEVICE_COUNT ?? 5),
  baseIntervalMs: Number(process.env.EMIT_INTERVAL_MS ?? 2000),
};
