CREATE TABLE IF NOT EXISTS telemetry_raw (
    id SERIAL PRIMARY KEY,
    device_id TEXT NOT NULL,
    timestamp TIMESTAMPTZ NOT NULL,
    latitude DOUBLE PRECISION NOT NULL,
    longitude DOUBLE PRECISION NOT NULL,
    speed DOUBLE PRECISION NOT NULL,
    battery DOUBLE PRECISION NOT NULL,
    temperature DOUBLE PRECISION NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_telemetry_device_id ON telemetry_raw(device_id);
CREATE INDEX IF NOT EXISTS idx_telemetry_timestamp ON telemetry_raw(timestamp);
